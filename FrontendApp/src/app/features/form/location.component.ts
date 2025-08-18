import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';
import { ApiService, DistrictGetterDto } from '../../core/api.service';

@Component({
	selector: 'app-location',
	standalone: true,
	imports: [ReactiveFormsModule, CommonModule, NgClass],
	templateUrl: './location.component.html',
})
export class LocationComponent implements OnInit {
	@Input({ required: true }) formGroup!: FormGroup;

	districts: DistrictGetterDto[] = [];
	filteredDistricts: DistrictGetterDto[] = [];
	showDistrictList = false;

	constructor(private api: ApiService) {}

	ngOnInit() {
		this.api.getDistricts().subscribe(districts => {
			this.districts = districts;
			this.filteredDistricts = [...this.districts];
		});
	}

	getInputValue(event: Event): string {
		return (event.target && (event.target as HTMLInputElement).value) || '';
	}

	onDistrictInput(value: string) {
		this.filteredDistricts = this.districts.filter(d =>
			d.name?.toLowerCase().includes((value || '').toLowerCase())
		);
		this.showDistrictList = this.filteredDistricts.length > 0 && value.length > 0;
	}

	selectDistrict(district: DistrictGetterDto) {
		this.formGroup.get('districtId')?.setValue(district.name);

		// Auto-completar otros campos relacionados
		if (district.zipCode) {
			this.formGroup.get('apartmentId')?.setValue(district.zipCode);
		}
		if (district.city) {
			this.formGroup.get('city')?.setValue(district.city);
		}
		if (district.country) {
			this.formGroup.get('country')?.setValue(district.country);
		}

		this.showDistrictList = false;
	}

	// Cerrar lista si se hace click fuera (opcional, mejora UX)
	// Puedes añadir un HostListener para click global si lo deseas
}
