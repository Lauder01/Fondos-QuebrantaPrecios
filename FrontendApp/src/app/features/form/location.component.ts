import { Component, Input } from '@angular/core';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';

@Component({
	selector: 'app-location',
	standalone: true,
	imports: [ReactiveFormsModule, CommonModule, NgClass],
	templateUrl: './location.component.html',
})
export class LocationComponent {
	@Input({ required: true }) formGroup!: FormGroup;

	districts: string[] = ['Centro', 'Salamanca', 'Retiro', 'Chamberí', 'Tetuán', 'Arganzuela', 'Usera', 'Carabanchel', 'Latina', 'Hortaleza', 'Barajas'];
	filteredDistricts: string[] = [...this.districts];
	showDistrictList = false;

	getInputValue(event: Event): string {
		return (event.target && (event.target as HTMLInputElement).value) || '';
	}

	onDistrictInput(value: string) {
		this.filteredDistricts = this.districts.filter(d => d.toLowerCase().includes((value || '').toLowerCase()));
		this.showDistrictList = this.filteredDistricts.length > 0 && value.length > 0;
	}

	selectDistrict(district: string) {
		this.formGroup.get('districtId')?.setValue(district);
		this.showDistrictList = false;
	}

	// Cerrar lista si se hace click fuera (opcional, mejora UX)
	// Puedes añadir un HostListener para click global si lo deseas
}
// ...existing code from original location will be moved here...
