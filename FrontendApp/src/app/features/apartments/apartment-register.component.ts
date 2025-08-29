import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Component } from '@angular/core';

@Component({
  selector: 'app-apartment-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './apartment-register.component.html'
})
export class ApartmentRegisterComponent {
  buildingId: string | null = null;
  step = 1;
  apartmentsPerFloor: number = 1;
  numRooms: number = 1;
  surface: number = 1;
  numBathrooms: number = 1;
  totalFloors: number = 1;
  currentFloor: number = 1;
  floors: number[] = [];

  constructor(private route: ActivatedRoute) {
    this.buildingId = this.route.snapshot.paramMap.get('buildingId');
  }

  confirmGeneralCharacteristics() {
    if (this.apartmentsPerFloor < 1) this.apartmentsPerFloor = 1;
    if (this.numRooms < 1) this.numRooms = 1;
    if (this.surface < 1) this.surface = 1;
    if (this.numBathrooms < 1) this.numBathrooms = 1;
    // Preguntar por el número de plantas
    const floorsInput = prompt('¿Cuántas plantas tiene el edificio? (Introduce un número entero)', '1');
    this.totalFloors = floorsInput ? Number(floorsInput) : 1;
    if (this.totalFloors < 1) this.totalFloors = 1;
    this.floors = Array(this.totalFloors).fill(0).map((_, i) => i + 1);
    this.currentFloor = 1;
    this.step = 2;
  }

  get apartmentsArray() {
    return Array(this.apartmentsPerFloor).fill(0).map((_, i) => i + 1);
  }

  goToFloor(floor: number) {
    this.currentFloor = floor;
  }
  prevFloor() {
    if (this.currentFloor > 1) this.currentFloor--;
  }
  nextFloor() {
    if (this.currentFloor < this.floors.length) this.currentFloor++;
  }
  finishRegistration() {
    // Aquí puedes implementar la lógica para finalizar el registro y navegar o mostrar un mensaje
    alert('Registro finalizado.');
  }
}
