
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BuildingCardComponent } from '../building-card/building-card.component';
import { BuildingService, Building } from './building.service';

@Component({
  selector: 'app-building-list',
  standalone: true,
  imports: [CommonModule, FormsModule, BuildingCardComponent],
  templateUrl: './building-list.component.html',
  styleUrl: './building-list.component.css'
})
export class BuildingListComponent implements OnInit {
  loading = false;
  buildings: Building[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 6;
  pages: number[] = [];
  searchName = '';

  constructor(private buildingService: BuildingService) {}

  ngOnInit(): void {
    this.fetchBuildings();
  }

  fetchBuildings() {
    this.loading = true;
    this.buildingService.getBuildings(this.page, this.pageSize, this.searchName).subscribe({
      next: (result) => {
        this.buildings = result.items;
        this.totalCount = result.totalCount;
        this.pages = Array.from({ length: Math.ceil(this.totalCount / this.pageSize) }, (_, i) => i + 1);
        this.loading = false;
      },
      error: () => {
        this.buildings = [];
        this.totalCount = 0;
        this.pages = [];
        this.loading = false;
      }
    });
  }

  onPageChange(newPage: number) {
    if (newPage < 1 || newPage > this.pages.length) return;
    this.page = newPage;
    this.fetchBuildings();
  }

  onPageSizeChange(event: any) {
    this.pageSize = Number(event);
    this.page = 1;
    this.fetchBuildings();
  }

  onSearchChange() {
    this.page = 1;
    this.fetchBuildings();
  }

  onCreateBuilding() {
    // Implementar navegación a formulario de creación si es necesario
  }
}
