import { Component, Input } from '@angular/core';
import { NgFor } from '@angular/common';
import { DistrictGetterDto } from '../../core/api.service';

@Component({
	selector: 'app-district-list',
	standalone: true,
	imports: [NgFor],
	template: `
		<h2>Distritos</h2>
		<ul>
			<li *ngFor="let district of districts">
				<strong>{{ district.name }}</strong> ({{ district.zipCode }})<br>
				<span>{{ district.city }}, {{ district.country }}</span>
			</li>
		</ul>
	`
})
export class DistrictListComponent {
	@Input() districts: DistrictGetterDto[] = [];
}
