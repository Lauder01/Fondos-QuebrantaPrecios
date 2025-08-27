import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BuildingCardComponent } from '../building-card/building-card.component';

@Component({
  selector: 'app-building-list',
  standalone: true,
  imports: [CommonModule, FormsModule, BuildingCardComponent],
  templateUrl: './building-list.component.html',
  styleUrl: './building-list.component.css'
})
export class BuildingListComponent {
  loading = false;
  allBuildings = [
    {
      id: 1,
      name: 'Edificio Central',
      district: 'Centro',
      address: 'Calle Mayor 123',
      price: 250000
    },
    {
      id: 2,
      name: 'Residencial Norte',
      district: 'Norte',
      address: 'Av. Libertad 45',
      price: 180000
    },
    {
      id: 3,
      name: 'Torre Sur',
      district: 'Sur',
      address: 'Paseo del Prado 8',
      price: 320000
    },
    {
      id: 4,
      name: 'Edificio Este',
      district: 'Este',
      address: 'Calle Sol 22',
      price: 210000
    },
    {
      id: 5,
      name: 'Residencial Oeste',
      district: 'Oeste',
      address: 'Av. Mar 10',
      price: 195000
    },
    {
      id: 6,
      name: 'Torre Norte',
      district: 'Norte',
      address: 'Paseo de la Paz 5',
      price: 330000
    }
  ];
  page = 1;
  pageSize = 6;
  pages: number[] = [];

  get buildings() {
    const start = (this.page - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.allBuildings.slice(start, end);
  }

  get totalCount() {
    return this.allBuildings.length;
  }

  constructor() {
    this.pages = Array.from({ length: Math.ceil(this.totalCount / this.pageSize) }, (_, i) => i + 1);
  }

  onPageChange(newPage: number) {
    if (newPage < 1 || newPage > this.pages.length) return;
    this.page = newPage;
    this.pages = Array.from({ length: Math.ceil(this.totalCount / this.pageSize) }, (_, i) => i + 1);
  }

  onPageSizeChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.pageSize = Number(value);
    this.page = 1;
    this.pages = Array.from({ length: Math.ceil(this.totalCount / this.pageSize) }, (_, i) => i + 1);
  }

  onCreateBuilding() {
  }
}
