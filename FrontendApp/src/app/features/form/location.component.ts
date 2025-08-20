import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiService, DistrictGetterDto, ZipcodeGetterDto, StreetGetterDto } from '../../core/api.service';

@Component({
    selector: 'app-location',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './location.component.html',
})

export class LocationComponent implements OnInit {
    @Input({ required: true }) formGroup!: FormGroup;

    allDistricts: DistrictGetterDto[] = [];
    allZipcodes: ZipcodeGetterDto[] = [];
    allStreets: StreetGetterDto[] = [];

    filteredDistricts: DistrictGetterDto[] = [];
    filteredZipcodes: ZipcodeGetterDto[] = [];
    filteredStreets: StreetGetterDto[] = [];

    showDistrictList = false;
    showZipcodeList = false;
    showStreetList = false;

    selectedDistrict: DistrictGetterDto | null = null;
    selectedZipcode: ZipcodeGetterDto | null = null;
    selectedStreet: StreetGetterDto | null = null;

    // Nuevas propiedades para controlar el modo de los campos
    districtMode: 'input' | 'dropdown' = 'input';
    zipcodeMode: 'input' | 'dropdown' = 'input';
    streetMode: 'input' | 'dropdown' = 'input';

    // Banderas para evitar bucles infinitos
    private isUpdatingDistrict = false;
    private isUpdatingZipcode = false;
    private isUpdatingStreet = false;

    // Estados de validación
    districtValid = true;
    zipcodeValid = true;
    streetValid = true;
    districtErrorMessage = '';
    zipcodeErrorMessage = '';
    streetErrorMessage = '';

    constructor(private api: ApiService) {}

    ngOnInit() {
        this.api.getDistricts().subscribe(districts => {
            this.allDistricts = districts;
            this.filteredDistricts = [...this.allDistricts];
        });
        this.api.getZipcodes().subscribe(zipcodes => {
            this.allZipcodes = zipcodes;
            this.filteredZipcodes = [...this.allZipcodes];
        });
        this.api.getStreets().subscribe(streets => {
            this.allStreets = streets;
            this.filteredStreets = [...this.allStreets];
        });
    }

    onDistrictInput(value: string) {
        if (!value) {
            this.resetDistrictSelection();
            return;
        }

        // Validar entrada: solo alfanuméricos y espacios, max 255 caracteres
        const sanitizedValue = this.sanitizeInput(value, 255);
        if (sanitizedValue !== value) {
            this.formGroup.get('districtId')?.setValue(sanitizedValue);
            return;
        }

        // Validar que el distrito existe
        const exactMatch = this.allDistricts.find(d =>
            d.name?.toLowerCase() === value.toLowerCase()
        );

        if (exactMatch) {
            // Distrito válido encontrado
            this.districtValid = true;
            this.districtErrorMessage = '';
            this.selectDistrict(exactMatch);
            return;
        }

        // Filtrar distritos disponibles (si hay zipcode seleccionado, solo los compatibles)
        const availableDistricts = this.selectedZipcode
            ? this.getDistrictsForZipcode(this.selectedZipcode.code)
            : this.allDistricts;

        this.filteredDistricts = availableDistricts.filter(d =>
            d.name?.toLowerCase().includes(value.toLowerCase())
        );

        // Validar si hay coincidencias
        if (this.filteredDistricts.length === 0) {
            this.districtValid = false;
            this.districtErrorMessage = 'No se encontraron distritos que coincidan';
        } else {
            this.districtValid = true;
            this.districtErrorMessage = '';
        }

        this.showDistrictList = this.filteredDistricts.length > 0;
    }    selectDistrict(district: DistrictGetterDto) {
        if (this.isUpdatingDistrict) return; // Evitar bucles infinitos

        this.selectedDistrict = district;
        this.formGroup.get('districtId')?.setValue(district.name);
        this.showDistrictList = false;

        // Auto-completar campos relacionados del distrito
        if (district.city) {
            this.formGroup.get('city')?.setValue(district.city);
        }
        if (district.country) {
            this.formGroup.get('country')?.setValue(district.country);
        }

        // Filtrar calles relacionadas con el distrito seleccionado
        console.log('Distrito seleccionado:', district);
        console.log('Calles disponibles:', this.allStreets);
        this.filteredStreets = this.allStreets.filter(street =>
            street.districts?.some(d => d.id === district.id)
        );
        console.log('Calles filtradas:', this.filteredStreets);

        // Lógica inteligente para códigos postales
        this.handleDistrictSelection(district);
    }

    resetDistrictSelection() {
        // Auto-borrado bidireccional: si zipcode tiene relación 1:1 con este distrito
        if (this.selectedZipcode && this.selectedDistrict) {
            const districtsForZipcode = this.getDistrictsForZipcode(this.selectedZipcode.code);
            if (districtsForZipcode.length === 1 && districtsForZipcode[0].id === this.selectedDistrict.id) {
                this.isUpdatingZipcode = true;
                this.selectedZipcode = null;
                this.formGroup.get('zipCode')?.setValue('');
                this.isUpdatingZipcode = false;
            }
        }

        this.selectedDistrict = null;
        this.formGroup.get('districtId')?.setValue('');
        this.showDistrictList = false;
        this.districtMode = 'input';
        this.districtValid = true;
        this.districtErrorMessage = '';

        // Restablecer el modo zipcode a input reactivo cuando se borra distrito
        this.zipcodeMode = 'input';

        // Si no hay zipcode seleccionado, mostrar todos los distritos y zipcodes
        if (!this.selectedZipcode) {
            this.filteredDistricts = [...this.allDistricts];
            this.filteredZipcodes = [...this.allZipcodes];
        } else {
            // Si hay zipcode seleccionado, NO ejecutar lógica inteligente automática
            // Solo mostrar distritos compatibles para búsqueda reactiva
            this.filteredDistricts = this.getDistrictsForZipcode(this.selectedZipcode.code);
            this.filteredZipcodes = [this.selectedZipcode]; // Solo mostrar el zipcode seleccionado
        }
    }

    onZipcodeInput(value: string) {
        if (!value) {
            this.resetZipcodeSelection();
            return;
        }

        // Validar entrada: solo números, max 5 caracteres
        const sanitizedValue = this.sanitizeZipcode(value);
        if (sanitizedValue !== value) {
            this.formGroup.get('zipCode')?.setValue(sanitizedValue);
            return;
        }

        // Validar que el código postal existe
        const exactMatch = this.allZipcodes.find(z => z.code === value);

        if (exactMatch) {
            // Código postal válido encontrado
            this.zipcodeValid = true;
            this.zipcodeErrorMessage = '';
            this.selectZipcode(exactMatch);
            return;
        }

        const availableZipcodes = this.selectedDistrict
            ? this.filteredZipcodes
            : this.allZipcodes;

        const filtered = availableZipcodes.filter(z =>
            z.code?.toLowerCase().includes(value.toLowerCase())
        );

        // Validar si hay coincidencias
        if (filtered.length === 0) {
            this.zipcodeValid = false;
            this.zipcodeErrorMessage = 'Código postal no válido';
        } else {
            this.zipcodeValid = true;
            this.zipcodeErrorMessage = '';
        }

        this.filteredZipcodes = filtered;
        this.showZipcodeList = filtered.length > 0;
    }

    selectZipcode(zipcode: ZipcodeGetterDto) {
        if (this.isUpdatingZipcode) return; // Evitar bucles infinitos

        this.selectedZipcode = zipcode;
        this.formGroup.get('zipCode')?.setValue(zipcode.code);
        this.showZipcodeList = false;

        // Lógica inteligente para distritos
        this.handleZipcodeSelection(zipcode);
    }

    resetZipcodeSelection() {
        // Auto-borrado bidireccional: si distrito tiene relación 1:1 con este zipcode
        if (this.selectedDistrict && this.selectedZipcode) {
            const zipcodesForDistrict = this.getZipcodesForDistrict(this.selectedDistrict);
            if (zipcodesForDistrict.length === 1 && zipcodesForDistrict[0].code === this.selectedZipcode.code) {
                this.isUpdatingDistrict = true;
                this.selectedDistrict = null;
                this.formGroup.get('districtId')?.setValue('');
                this.isUpdatingDistrict = false;
            }
        }

        this.selectedZipcode = null;
        this.formGroup.get('zipCode')?.setValue('');
        this.showZipcodeList = false;
        this.zipcodeMode = 'input';
        this.zipcodeValid = true;
        this.zipcodeErrorMessage = '';

        // Restablecer el modo distrito a input reactivo cuando se borra zipcode
        this.districtMode = 'input';

        // Si no hay distrito seleccionado, mostrar todos los códigos postales y distritos
        if (!this.selectedDistrict) {
            this.filteredZipcodes = [...this.allZipcodes];
            this.filteredDistricts = [...this.allDistricts];
        } else {
            // Si hay distrito seleccionado, NO ejecutar lógica inteligente automática
            // Solo mostrar zipcodes compatibles para búsqueda reactiva
            this.filteredZipcodes = this.getZipcodesForDistrict(this.selectedDistrict);
            this.filteredDistricts = [this.selectedDistrict]; // Solo mostrar el distrito seleccionado
        }
    }

    onStreetInput(value: string) {
        if (!value) {
            this.filteredStreets = [];
            this.showStreetList = false;
            return;
        }

        // Filtrar calles disponibles basándose en el valor ingresado
        this.filteredStreets = this.allStreets.filter(street =>
            street.name.toLowerCase().includes(value.toLowerCase())
        );

        this.showStreetList = this.filteredStreets.length > 0;
    }

    selectStreet(street: StreetGetterDto) {
        this.selectedStreet = street;
        this.formGroup.get('streetId')?.setValue(street.name);
        this.showStreetList = false;
    }

    // ==================== MÉTODOS AUXILIARES ====================

    private getZipcodesForDistrict(district: DistrictGetterDto): ZipcodeGetterDto[] {
        if (!district.zipcodes || district.zipcodes.length === 0) {
            return [];
        }

        return this.allZipcodes.filter(zipcode =>
            district.zipcodes!.includes(zipcode.code)
        );
    }

    private getDistrictsForZipcode(zipcodeCode: string): DistrictGetterDto[] {
        return this.allDistricts.filter(district =>
            district.zipcodes?.includes(zipcodeCode)
        );
    }

    // ==================== LÓGICA INTELIGENTE ====================

    private handleDistrictSelection(district: DistrictGetterDto) {
        const availableZipcodes = this.getZipcodesForDistrict(district);

        if (availableZipcodes.length === 1) {
            // Si hay un solo código postal, autocompletarlo
            const zipcode = availableZipcodes[0];
            this.isUpdatingZipcode = true; // Evitar bucle
            this.selectedZipcode = zipcode;
            this.formGroup.get('zipCode')?.setValue(zipcode.code);
            this.zipcodeMode = 'input';
            this.filteredZipcodes = [zipcode];
            this.isUpdatingZipcode = false;
        } else if (availableZipcodes.length > 1) {
            // Si hay múltiples códigos postales, cambiar a modo dropdown
            this.zipcodeMode = 'dropdown';
            this.filteredZipcodes = availableZipcodes;
            // Limpiar selección actual si no es compatible
            if (this.selectedZipcode && !availableZipcodes.find(z => z.code === this.selectedZipcode!.code)) {
                this.isUpdatingZipcode = true; // Evitar bucle
                this.selectedZipcode = null;
                this.formGroup.get('zipCode')?.setValue('');
                this.isUpdatingZipcode = false;
            }
        } else {
            // No hay códigos postales disponibles
            this.zipcodeMode = 'input';
            this.filteredZipcodes = [];
            this.isUpdatingZipcode = true; // Evitar bucle
            this.selectedZipcode = null;
            this.formGroup.get('zipCode')?.setValue('');
            this.isUpdatingZipcode = false;
        }
    }

    private handleZipcodeSelection(zipcode: ZipcodeGetterDto) {
        const availableDistricts = this.getDistrictsForZipcode(zipcode.code);

        if (availableDistricts.length === 1) {
            // Si hay un solo distrito, autocompletarlo
            const district = availableDistricts[0];
            this.isUpdatingDistrict = true; // Evitar bucle
            this.selectedDistrict = district;
            this.formGroup.get('districtId')?.setValue(district.name);

            // Auto-completar campos relacionados del distrito
            if (district.city) {
                this.formGroup.get('city')?.setValue(district.city);
            }
            if (district.country) {
                this.formGroup.get('country')?.setValue(district.country);
            }

            this.districtMode = 'input';
            this.filteredDistricts = [district];
            this.isUpdatingDistrict = false;
        } else if (availableDistricts.length > 1) {
            // Si hay múltiples distritos, cambiar a modo dropdown
            this.districtMode = 'dropdown';
            this.filteredDistricts = availableDistricts;
            // Limpiar selección actual si no es compatible
            if (this.selectedDistrict && !availableDistricts.find(d => d.id === this.selectedDistrict!.id)) {
                this.isUpdatingDistrict = true; // Evitar bucle
                this.selectedDistrict = null;
                this.formGroup.get('districtId')?.setValue('');
                this.isUpdatingDistrict = false;
            }
        } else {
            // No hay distritos disponibles
            this.districtMode = 'input';
            this.filteredDistricts = [];
            this.isUpdatingDistrict = true; // Evitar bucle
            this.selectedDistrict = null;
            this.formGroup.get('districtId')?.setValue('');
            this.isUpdatingDistrict = false;
        }
    }

    getInputValue(event: Event): string {
        return (event.target && (event.target as HTMLInputElement).value) || '';
    }

    // ==================== MÉTODOS DE VALIDACIÓN ====================

    private sanitizeInput(value: string, maxLength: number): string {
        // Solo permitir caracteres alfanuméricos, espacios, guiones y acentos
        const sanitized = value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s\-]/g, '');
        return sanitized.length > maxLength ? sanitized.substring(0, maxLength) : sanitized;
    }

    private sanitizeZipcode(value: string): string {
        // Solo números, máximo 5 caracteres para códigos postales españoles
        const sanitized = value.replace(/[^0-9]/g, '');
        return sanitized.length > 5 ? sanitized.substring(0, 5) : sanitized;
    }

    // ==================== MÉTODOS PARA DROPDOWNS ====================

    onDistrictDropdownChange(event: Event) {
        const select = event.target as HTMLSelectElement;
        const districtName = select.value;

        if (districtName) {
            const district = this.filteredDistricts.find(d => d.name === districtName);
            if (district) {
                this.selectDistrict(district);
            }
        } else {
            this.resetDistrictSelection();
        }
    }

    onZipcodeDropdownChange(event: Event) {
        const select = event.target as HTMLSelectElement;
        const zipcodeCode = select.value;

        if (zipcodeCode) {
            const zipcode = this.filteredZipcodes.find(z => z.code === zipcodeCode);
            if (zipcode) {
                this.selectZipcode(zipcode);
            }
        } else {
            this.resetZipcodeSelection();
        }
    }
}
