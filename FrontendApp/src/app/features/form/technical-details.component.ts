import { Component, Input } from '@angular/core';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';

@Component({
	selector: 'app-technical-details',
	standalone: true,
	imports: [ReactiveFormsModule, CommonModule, NgClass],
	templateUrl: './technical-details.component.html',
})
export class TechnicalDetailsComponent {
	@Input({ required: true }) formGroup!: FormGroup;
}
// ...existing code from original location will be moved here...
