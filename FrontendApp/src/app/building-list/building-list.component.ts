import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BuildingService, Building, BuildingListResult } from './building.service';

@Component({
  selector: 'app-building-list',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, FormsModule],
  templateUrl: './building-list.component.html',
  styleUrl: './building-list.component.css'
})
export class BuildingListComponent implements OnInit {
  buildings: Building[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 6;
  name = '';
  districtId = '';
  companyId = '';
  loading = false;
  error = false;
  pages: number[] = [];

  constructor(private buildingService: BuildingService, private router: Router) {}

  ngOnInit() {
    this.getBuildings();
  }

  getBuildings() {
    this.loading = true;
    this.error = false;
    this.buildingService.getBuildings(this.page, this.pageSize, this.name, this.districtId, this.companyId)
      .subscribe({
        next: (result) => {
          this.buildings = result.items;
          this.totalCount = result.totalCount;
          this.pages = Array.from({ length: Math.ceil(this.totalCount / this.pageSize) }, (_, i) => i + 1);
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.error = true;
          this.buildings = [];
        }
      });
  }

  onSearchInput(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.name = value;
    this.page = 1;
    this.getBuildings();
  }

  onPageChange(newPage: number) {
    if (newPage < 1 || newPage > this.pages.length) return;
    this.page = newPage;
    this.getBuildings();
  }

  onCreateBuilding() {
    this.router.navigate(['/form']);
  }
}
