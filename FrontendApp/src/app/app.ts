import { Component, signal } from '@angular/core';
import { DistrictListComponent } from './district-list.component';
import { ApiService, DistrictGetterDto } from './api.service';

@Component({
  selector: 'app-root',
  imports: [DistrictListComponent],
  template: `<app-district-list [districts]="districts"></app-district-list>`,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('FrontendApp');
  districts: DistrictGetterDto[] = [];

  constructor(private apiService: ApiService) {
    this.apiService.getDistricts().subscribe((data: DistrictGetterDto[]) => {
      this.districts = data;
    });
  }
}
