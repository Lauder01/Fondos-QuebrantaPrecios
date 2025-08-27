
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-apartment-register',
  standalone: true,
  templateUrl: './apartment-register.component.html'
})
export class ApartmentRegisterComponent {
  buildingId: string | null = null;
  step = 1;
  apartmentsPerFloor: number = 1;
  totalFloors: number = 1;
  currentFloor: number = 1;
  floors: number[] = [];

  constructor(private route: ActivatedRoute) {
    this.buildingId = this.route.snapshot.paramMap.get('buildingId');
    // Aquí podrías obtener el número real de plantas del edificio si lo tienes
    // Por ahora, lo dejamos a 1 hasta que el usuario lo indique
  }

  confirmApartmentsPerFloor() {
    if (this.apartmentsPerFloor < 1) this.apartmentsPerFloor = 1;
    // Aquí podrías preguntar también por el número de plantas, o traerlo de la API
    // Para el ejemplo, preguntamos por el número de plantas
    this.totalFloors = prompt('¿Cuántas plantas tiene el edificio? (Introduce un número entero)', '1') ? Number(prompt('¿Cuántas plantas tiene el edificio? (Introduce un número entero)', '1')) : 1;
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
}
