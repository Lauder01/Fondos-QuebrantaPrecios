import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-building-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './building-detail.component.html',
  styleUrl: './building-detail.component.css'
})
export class BuildingDetailComponent {
  buildingId: number | null = null;
  building: any = null;

  // Simulación de datos, igual que en BuildingListComponent
  allBuildings = [
    {
      id: 1,
      name: 'Edificio Central',
      district: 'Centro',
      address: 'Calle Mayor 123',
      price: 250000,
      description: 'Edificio emblemático en el centro de la ciudad.',
      yearBuilt: 1990,
      floorCount: 10,
      hasElevator: true,
      energyCertificate: 'A'
    },
    {
      id: 2,
      name: 'Residencial Norte',
      district: 'Norte',
      address: 'Av. Libertad 45',
      price: 180000,
      description: 'Residencial moderno en zona norte.',
      yearBuilt: 2005,
      floorCount: 8,
      hasElevator: true,
      energyCertificate: 'B'
    },
    {
      id: 3,
      name: 'Torre Sur',
      district: 'Sur',
      address: 'Paseo del Prado 8',
      price: 320000,
      description: 'Torre con vistas al sur de la ciudad.',
      yearBuilt: 2010,
      floorCount: 15,
      hasElevator: true,
      energyCertificate: 'A'
    },
    {
      id: 4,
      name: 'Edificio Este',
      district: 'Este',
      address: 'Calle Sol 22',
      price: 210000,
      description: 'Edificio luminoso en el este.',
      yearBuilt: 2000,
      floorCount: 7,
      hasElevator: false,
      energyCertificate: 'C'
    },
    {
      id: 5,
      name: 'Residencial Oeste',
      district: 'Oeste',
      address: 'Av. Mar 10',
      price: 195000,
      description: 'Residencial tranquilo en el oeste.',
      yearBuilt: 1995,
      floorCount: 6,
      hasElevator: false,
      energyCertificate: 'B'
    },
    {
      id: 6,
      name: 'Torre Norte',
      district: 'Norte',
      address: 'Paseo de la Paz 5',
      price: 330000,
      description: 'Torre moderna en el norte.',
      yearBuilt: 2015,
      floorCount: 20,
      hasElevator: true,
      energyCertificate: 'A'
    }
  ];

  constructor(private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.buildingId = Number(params.get('id'));
      this.building = this.allBuildings.find(b => b.id === this.buildingId);
    });
  }
}
