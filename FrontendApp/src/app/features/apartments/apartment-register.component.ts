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
  area: number = 1;
  numBathrooms: number = 1;
  totalFloors: number = 1;
  currentFloor: number = 1;
  floors: number[] = [];
  buildingFloors: FloorGetterDto[] = [];
  isCreatingApartments = false;

  // Estructura para apartamentos personalizados por planta
  apartmentsByFloor: { [floorId: string]: ApartmentCreatorDto[] } = {};

  // Método para obtener el floorId de la planta activa
  getCurrentFloorId(): string {
    const floor = this.buildingFloors.find(f => f.floorNumber === this.currentFloor);
    return floor ? floor.id : '';
  }

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
    // Inicializar apartamentos personalizados por planta con los valores del formulario general
    if (this.buildingFloors.length > 0) {
      this.step = 2;
      let globalAptIndex = 1;
      this.buildingFloors.forEach(floor => {
        this.apartmentsByFloor[floor.id] = [];
        for (let i = 0; i < this.apartmentsPerFloor; i++) {
          this.apartmentsByFloor[floor.id].push({
            code: `APT-${globalAptIndex.toString().padStart(3, '0')}`,
            door: `${i + 1}${this.getDoorLetter(i + 1)}`,
            floorId: floor.id,
            numRooms: this.numRooms && !isNaN(this.numRooms) ? Number(this.numRooms) : 1,
            numBathrooms: this.numBathrooms && !isNaN(this.numBathrooms) ? Number(this.numBathrooms) : 1,
            area: this.area && !isNaN(this.area) ? Number(this.area) : 70
          });
          globalAptIndex++;
        }
      });
      this.currentFloor = this.buildingFloors[0].floorNumber;
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
    // Siempre inicializar apartamentos si no existen
    if (Object.keys(this.apartmentsByFloor).length === 0 && this.buildingFloors.length > 0) {
      let globalAptIndex = 1;
      this.buildingFloors.forEach(floor => {
        this.apartmentsByFloor[floor.id] = [];
        for (let i = 0; i < this.apartmentsPerFloor; i++) {
          this.apartmentsByFloor[floor.id].push({
            code: `APT-${globalAptIndex.toString().padStart(3, '0')}`,
            door: `${i + 1}${this.getDoorLetter(i + 1)}`,
            floorId: floor.id,
            numRooms: this.numRooms && !isNaN(this.numRooms) ? Number(this.numRooms) : 0,
            numBathrooms: this.numBathrooms && !isNaN(this.numBathrooms) ? Number(this.numBathrooms) : 0,
            area: this.area && !isNaN(this.area) ? Number(this.area) : 0
          });
          globalAptIndex++;
        }
      });
    }
    // Si los apartamentos no están inicializados, inicializarlos automáticamente (como en confirmGeneralCharacteristics)
    if (Object.keys(this.apartmentsByFloor).length === 0 && this.buildingFloors.length > 0) {
      this.buildingFloors.forEach(floor => {
        this.apartmentsByFloor[floor.id] = [];
        for (let i = 0; i < this.apartmentsPerFloor; i++) {
          this.apartmentsByFloor[floor.id].push({
            code: `${floor.floorNumber}-${(i + 1).toString().padStart(2, '0')}`,
            door: `${i + 1}${this.getDoorLetter(i + 1)}`,
            floorId: floor.id,
            numRooms: this.numRooms && !isNaN(this.numRooms) ? Number(this.numRooms) : 1,
            numBathrooms: this.numBathrooms && !isNaN(this.numBathrooms) ? Number(this.numBathrooms) : 1,
            area: this.area && !isNaN(this.area) ? Number(this.area) : 70
          });
        }
      });
    }
    if (!this.buildingId) {
      alert('Error: No se ha encontrado el ID del edificio');
      return;
    }

    if (this.isCreatingApartments) {
      return; // Evitar múltiples clics
    }

    // Validar y limpiar datos antes de enviar
    const apartmentPayloads: any[] = [];
    const apartmentCreationRequests: any[] = [];
    for (const floorId in this.apartmentsByFloor) {
      for (const apt of this.apartmentsByFloor[floorId]) {
        // Limpiar y convertir todos los campos
        const payload = {
          code: String(apt.code ?? '').trim(),
          door: String(apt.door ?? '').trim(),
          floorId: String(apt.floorId ?? '').trim(),
          numRooms: Number(apt.numRooms),
          numBathrooms: Number(apt.numBathrooms),
          area: Number(apt.area)
        };
        // Validar campos obligatorios y área válida
        if (!payload.code || !payload.door || !payload.floorId) {
          alert('Por favor, completa todos los campos requeridos en todos los apartamentos.');
          return;
        }
        if (isNaN(payload.area) || payload.area <= 0) {
          alert('El área de cada apartamento debe ser mayor que 0. Corrige los valores antes de continuar.');
          return;
        }
        apartmentPayloads.push(payload);
        apartmentCreationRequests.push(this.api.createApartment(payload));
      }
    }

    // Log para depuración: mostrar los datos que se envían a la API
    console.log('Datos de apartamentos enviados a la API:', JSON.stringify(apartmentPayloads, null, 2));

    this.isCreatingApartments = true;

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

  addApartment(floorId: string) {
    const apts = this.apartmentsByFloor[floorId];
    if (apts && apts.length > 0) {
      // Copiar datos del primer apartamento
      const base = { ...apts[0] };
      // Generar nuevo código y puerta
      const newIndex = apts.length;
      base.code = `${this.currentFloor}-${(newIndex + 1).toString().padStart(2, '0')}`;
      base.door = `${newIndex + 1}${this.getDoorLetter(newIndex + 1)}`;
      apts.push({ ...base });
    } else if (apts) {
      // Si no hay ninguno, crear uno básico
      apts.push({
        code: `${this.currentFloor}-01`,
        door: `1${this.getDoorLetter(1)}`,
        floorId: floorId,
        numRooms: 1,
        numBathrooms: 1,
        area: 70
      });
    }
  }

  removeApartment(floorId: string, index: number) {
    const apts = this.apartmentsByFloor[floorId];
    if (apts && apts.length > index) {
      apts.splice(index, 1);
    }
  }
}
