
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GeneralInfoComponent } from './general-info.component';
import { LocationComponent } from './location.component';
import { TechnicalDetailsComponent } from './technical-details.component';
import { ApiService, BuildingCreatorDto } from '../../core/api.service';

@Component({
	selector: 'app-form-main',
	standalone: true,
	imports: [ReactiveFormsModule, GeneralInfoComponent, LocationComponent, TechnicalDetailsComponent],
	templateUrl: './form-main.component.html',
})
export class FormMainComponent {
	form: FormGroup;

	constructor(private fb: FormBuilder, private api: ApiService, private router: Router) {
		this.form = this.fb.group({
			generalInfo: this.fb.group({
				name: [''],
				description: [''],
				buildingCompanyId: ['', Validators.required]
			}),

			location: this.fb.group({
				districtId: ['', Validators.required],
				districtName: [''], // Campo auxiliar para mostrar el nombre
				zipCode: ['', Validators.required],
				streetId: ['', Validators.required],
				streetName: [''], // Campo auxiliar para mostrar el nombre
				buildingNumber: ['', Validators.required],
				city: ['', Validators.required],
				country: ['', Validators.required]
			}),

			technicalDetails: this.fb.group({
				floorCount: [1, [Validators.required, Validators.min(1)]],
				yearBuilt: [2025, [Validators.required, Validators.min(1800), Validators.max(new Date().getFullYear()+1)]],
				price: [0, [Validators.required, Validators.min(0)]],
        energyCertificate: ['', Validators.required],
				hasElevator: [false]
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
			// No necesitamos buscar el status, se asigna automáticamente en el backend
			const formData = this.form.value;

			const buildingData: BuildingCreatorDto = {
				// Información General - name siempre se envía, aunque esté vacío
				name: (formData.generalInfo.name || '').trim(),
				description: (formData.generalInfo.description || '').trim(),
				buildingCompanyId: (formData.generalInfo.buildingCompanyId || '').trim(),
				// statusId se asigna automáticamente en el backend

				// Ubicación
				districtId: (formData.location.districtId || '').trim(),
				streetId: (formData.location.streetId || '').trim(),
				doorway: (formData.location.buildingNumber || '').trim(),

				// Detalles Técnicos
				floorCount: Number(formData.technicalDetails.floorCount) || 1,
				yearBuilt: Number(formData.technicalDetails.yearBuilt) || 2025,
				price: formData.technicalDetails.price !== undefined && formData.technicalDetails.price !== null ? String(formData.technicalDetails.price) : '0',
				energyCertificate: (formData.technicalDetails.energyCertificate || '').trim(),
				hasElevator: !!formData.technicalDetails.hasElevator

				// El código se genera automáticamente en el backend usando BuildBuildingCode()
			};

			// Eliminar campos undefined/null
			Object.keys(buildingData).forEach(key => {
				if (buildingData[key as keyof BuildingCreatorDto] === undefined || buildingData[key as keyof BuildingCreatorDto] === null) {
					delete buildingData[key as keyof BuildingCreatorDto];
				}
			});

			console.log('Enviando edificio:', buildingData);

			// Enviar a la API
					this.api.createBuilding(buildingData).subscribe({
						next: (result) => {
							console.log('Edificio creado exitosamente:', result);
							alert(`Edificio "${result.name || 'Sin nombre'}" registrado exitosamente con ID: ${result.id}`);

							// Redirigir a la página de registro de apartamentos
							if (result.id) {
								this.router.navigate(['/apartments/register', result.id]);
							}

							// Opcional: Limpiar el formulario
							this.resetForm();
						},
				error: (error) => {
					console.error('Error al crear el edificio:', error);
					let errorMessage = 'Error al registrar el edificio.';

					if (error.error && error.error.errors) {
						// Errores de validación del servidor
						const validationErrors = Object.values(error.error.errors).flat();
						errorMessage = `Errores de validación:\n${validationErrors.join('\n')}`;
					} else if (error.error && error.error.message) {
						errorMessage = error.error.message;
					}

					alert(errorMessage);
				}
			});
		} else {
			console.log('Formulario inválido');
			this.markAllFieldsAsTouched();
			alert('Por favor, complete todos los campos obligatorios.');
		}
	}

	private resetForm() {
		this.form.reset();
		// Restablecer valores por defecto
		this.form.patchValue({
			technicalDetails: {
				floorCount: 1,
				yearBuilt: 2025,
				price: 0,
				hasElevator: false
			}
		});
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
