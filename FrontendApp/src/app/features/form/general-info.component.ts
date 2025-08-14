import { Component, Input } from '@angular/core';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';

@Component({
	selector: 'app-general-info',
	standalone: true,
	imports: [ReactiveFormsModule, CommonModule, NgClass],
	templateUrl: './general-info.component.html',
})
export class GeneralInfoComponent {
	@Input({ required: true }) formGroup!: FormGroup;
}
// ...existing code from original location will be moved here...
