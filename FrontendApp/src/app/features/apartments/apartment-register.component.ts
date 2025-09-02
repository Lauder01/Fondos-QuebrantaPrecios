import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ApiService, FloorGetterDto, ApartmentCreatorDto } from '../../core/api.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-apartment-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './apartment-register.component.html'
})
export class ApartmentRegisterComponent implements OnInit {
  buildingId: string | null = null;
  step = 1;
  apartmentsPerFloor: number = 1;
  numRooms: number = 1;
  surface: number = 1;
  numBathrooms: number = 1;
  totalFloors: number = 1;
  currentFloor: number = 1;
  floors: number[] = [];
  buildingFloors: FloorGetterDto[] = [];
  isCreatingApartments = false;

  constructor(private route: ActivatedRoute, private api: ApiService, private router: Router) {
    this.buildingId = this.route.snapshot.paramMap.get('buildingId');
  }

  ngOnInit() {
    if (this.buildingId) {
      // Cargar los floors del edificio
      this.api.getFloorsByBuildingId(this.buildingId).subscribe({
        next: (floors) => {
          this.buildingFloors = floors.sort((a, b) => a.floorNumber - b.floorNumber);
          this.totalFloors = floors.length;
          this.floors = floors.map(f => f.floorNumber);
          if (this.floors.length > 0) {
            this.currentFloor = this.floors[0];
          }
        },
        error: (error) => {
          console.error('Error al cargar los floors del edificio:', error);
          alert('Error al cargar los pisos del edificio');
        }
      });
    }
  }

  confirmGeneralCharacteristics() {
    if (this.apartmentsPerFloor < 1) this.apartmentsPerFloor = 1;
    if (this.numRooms < 1) this.numRooms = 1;
    if (this.surface < 1) this.surface = 1;
    if (this.numBathrooms < 1) this.numBathrooms = 1;

    // Usar los floors ya cargados del edificio
    if (this.buildingFloors.length > 0) {
      this.step = 2;
    } else {
      alert('No se han encontrado pisos para este edificio');
    }
  }

  get apartmentsArray() {
    return Array(this.apartmentsPerFloor).fill(0).map((_, i) => i + 1);
  }

  goToFloor(floor: number) {
    this.currentFloor = floor;
  }

  prevFloor() {
    const currentIndex = this.floors.indexOf(this.currentFloor);
    if (currentIndex > 0) {
      this.currentFloor = this.floors[currentIndex - 1];
    }
  }

  nextFloor() {
    const currentIndex = this.floors.indexOf(this.currentFloor);
    if (currentIndex < this.floors.length - 1) {
      this.currentFloor = this.floors[currentIndex + 1];
    }
  }

  finishRegistration() {
    if (!this.buildingId) {
      alert('Error: No se ha encontrado el ID del edificio');
      return;
    }

    if (this.isCreatingApartments) {
      return; // Evitar múltiples clics
    }

    this.isCreatingApartments = true;

    // Crear todos los apartamentos
    const apartmentCreationRequests: any[] = [];

    this.buildingFloors.forEach(floor => {
      for (let aptNumber = 1; aptNumber <= this.apartmentsPerFloor; aptNumber++) {
        const apartmentCode = `${floor.floorNumber}-${aptNumber.toString().padStart(2, '0')}`;
        const apartmentDoor = `${aptNumber}${this.getDoorLetter(aptNumber)}`; // 1A, 2B, etc.

        const apartmentData: ApartmentCreatorDto = {
          code: apartmentCode,
          door: apartmentDoor,
          floorId: floor.id
        };

        apartmentCreationRequests.push(this.api.createApartment(apartmentData));
      }
    });

    // Ejecutar todas las creaciones en paralelo
    forkJoin(apartmentCreationRequests).subscribe({
      next: (results) => {
        console.log('Apartamentos creados exitosamente:', results);
        alert(`Se han creado ${results.length} apartamentos exitosamente`);
        this.router.navigate(['/buildings']); // Navegar de vuelta a la lista de edificios
      },
      error: (error) => {
        console.error('Error al crear apartamentos:', error);
        alert('Error al crear los apartamentos. Por favor, inténtelo de nuevo.');
        this.isCreatingApartments = false;
      },
      complete: () => {
        this.isCreatingApartments = false;
      }
    });
  }

  // Helper method para generar letras para las puertas
  getDoorLetter(aptNumber: number): string {
    return String.fromCharCode(64 + aptNumber); // A, B, C, etc.
  }
}
