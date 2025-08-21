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

	getEnergyClass(value: string): string {
		if (!value) return 'energy-default';

		switch (value.toUpperCase()) {
			case 'A': return 'energy-a';
			case 'B': return 'energy-b';
			case 'C': return 'energy-c';
			case 'D': return 'energy-d';
			case 'E': return 'energy-e';
			case 'F': return 'energy-f';
			case 'G': return 'energy-g';
			default: return 'energy-default';
		}
	}
}
// ...existing code from original location will be moved here...
