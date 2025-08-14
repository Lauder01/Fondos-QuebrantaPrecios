import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, DistrictGetterDto } from '../../core/api.service';
import { DistrictListComponent } from './district-list.component';

@Component({
	selector: 'app-districts-page',
	standalone: true,
	imports: [CommonModule, DistrictListComponent],
	template: `
		<h1>Distritos</h1>
		<app-district-list [districts]="districts"></app-district-list>
	`
})
export class DistrictsPageComponent implements OnInit {
	districts: DistrictGetterDto[] = [];
	constructor(private api: ApiService) {}
	ngOnInit() {
		this.api.getDistricts().subscribe(d => this.districts = d);
	}
}
// ...existing code from original location will be moved here...
