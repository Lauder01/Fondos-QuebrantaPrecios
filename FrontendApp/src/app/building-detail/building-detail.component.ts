
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BuildingService, Building } from '../building-list/building.service';

@Component({
  selector: 'app-building-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './building-detail.component.html',
  styleUrl: './building-detail.component.css'
})
export class BuildingDetailComponent implements OnInit {
  buildingId: string | null = null;
  building: Building | null = null;
  loading = false;


  readonly SOLD_STATUS_ID = 'CB85B731-5FBC-4F56-9938-E183B51EE409';

  constructor(private route: ActivatedRoute, private buildingService: BuildingService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.buildingId = params.get('id');
      if (this.buildingId) {
        this.fetchBuilding(this.buildingId);
      }
    });
  }

  fetchBuilding(id: string) {
    this.loading = true;
    this.buildingService.getBuildings(1, 1, undefined, undefined, undefined).subscribe({
      next: (result) => {
        // Si la API tuviera un endpoint específico para obtener por ID, usarlo aquí
        // Pero el servicio actual solo tiene getBuildings (paginado), así que buscamos en el resultado
        const found = result.items.find(b => b.id === id);
        this.building = found || null;
        this.loading = false;
      },
      error: () => {
        this.building = null;
        this.loading = false;
      }
    });
  }

  buyBuilding() {
    if (!this.buildingId) return;
    this.buildingService.updateBuildingStatus(this.buildingId, this.SOLD_STATUS_ID).subscribe({
      next: () => {
        alert('¡Edificio comprado exitosamente!');
        this.fetchBuilding(this.buildingId!); // refrescar datos
      },
      error: () => {
        alert('Error al comprar el edificio.');
      }
    });
  }
}
