
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { GeneralInfoComponent } from './general-info.component';
import { LocationComponent } from './location.component';
import { TechnicalDetailsComponent } from './technical-details.component';

@Component({
	selector: 'app-form-main',
	standalone: true,
	imports: [ReactiveFormsModule, GeneralInfoComponent, LocationComponent, TechnicalDetailsComponent],
	templateUrl: './form-main.component.html',
})
export class FormMainComponent {
	form: FormGroup;
		constructor(private fb: FormBuilder) {
			this.form = this.fb.group({
				generalInfo: this.fb.group({
					name: ['', [Validators.required, Validators.minLength(3)]],
					description: ['', [Validators.required, Validators.minLength(10)]],
					code: ['', [Validators.required, Validators.pattern(/^([A-Z0-9]{8,12})$/)]],
					doorway: ['', Validators.required],
					buildingCompanyId: ['', Validators.required],
					statusId: ['', Validators.required]
				}),
				location: this.fb.group({
					districtId: ['', Validators.required],
					streetId: ['', Validators.required],
					doorway: ['', Validators.required],
					apartmentId: ['', Validators.required],
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
		console.log(this.form.value);
	}
}
// ...existing code from original location will be moved here...
