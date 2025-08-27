import { Component } from '@angular/core';
import { BuildingCardComponent } from '../building-card/building-card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-landing',
  standalone: true,
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
  imports: [CommonModule, BuildingCardComponent]
})
export class LandingComponent {
  buildings = [
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
    }
  ];
}
