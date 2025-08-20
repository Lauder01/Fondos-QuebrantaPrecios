
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { GeneralInfoComponent } from './general-info.component';
import { LocationComponent } from './location.component';
import { TechnicalDetailsComponent } from './technical-details.component';
import { ApiService } from '../../core/api.service';

@Component({
	selector: 'app-form-main',
	standalone: true,
	imports: [ReactiveFormsModule, GeneralInfoComponent, LocationComponent, TechnicalDetailsComponent],
	templateUrl: './form-main.component.html',
})
export class FormMainComponent {
	form: FormGroup;

	constructor(private fb: FormBuilder, private api: ApiService) {
		this.form = this.fb.group({
			generalInfo: this.fb.group({
				name: ['', [Validators.required, Validators.minLength(3)]],
				description: ['', [Validators.required, Validators.minLength(10)]],
				code: ['', [Validators.required, Validators.pattern(/^([A-Z0-9]{8,12})$/)]],
				buildingCompanyId: ['', Validators.required]
			}),

			location: this.fb.group({
				districtId: ['', Validators.required],
				zipCode: ['', Validators.required],
				streetId: ['', Validators.required],
				buildingNumber: ['', Validators.required],
				city: ['', Validators.required],
				country: ['', Validators.required]
			}),

			technicalDetails: this.fb.group({
				floorCount: [1, [Validators.required, Validators.min(1)]],
				yearBuilt: [2025, [Validators.required, Validators.min(1800), Validators.max(new Date().getFullYear()+1)]],
				price: [0, [Validators.required, Validators.min(0)]],
				apartmentsPerFloor: [1, [Validators.required, Validators.min(1)]],
				hasElevator: [false],
				energyCertificate: ['']
			})
		});
	}

	get generalInfoGroup() {
		return this.form.get('generalInfo') as FormGroup;
	}

	get locationGroup() {
		return this.form.get('location') as FormGroup;
	}

	get technicalDetailsGroup() {
		return this.form.get('technicalDetails') as FormGroup;
	}

	onSubmit() {
		if (this.form.valid) {
			// Buscar el status "registrado" por nombre
			this.api.getStatusByName('registrado').subscribe(status => {
				if (status) {
					const formData = { ...this.form.value };

					formData.generalInfo.statusId = status.id;
					formData.registeredAt = new Date().toISOString();

					console.log('Edificio registrado:', formData);
					console.log('Status aplicado:', status);

					alert('Edificio registrado exitosamente con status: ' + status.name);
				} else {
					console.error('No se encontró el status "registrado"');
					alert('Error: No se pudo encontrar el status "registrado". Contacte al administrador.');
				}
			}, error => {
				console.error('Error al obtener el status:', error);
				alert('Error al obtener el status del edificio. Inténtelo de nuevo.');
			});
		} else {
			console.log('Formulario inválido');
			this.markAllFieldsAsTouched();
		}
	}

	private markAllFieldsAsTouched() {
		Object.keys(this.form.controls).forEach(key => {
			const control = this.form.get(key);
			if (control instanceof FormGroup) {
				Object.keys(control.controls).forEach(subKey => {
					control.get(subKey)?.markAsTouched();
				});
			} else {
				control?.markAsTouched();
			}
		});
	}
}
