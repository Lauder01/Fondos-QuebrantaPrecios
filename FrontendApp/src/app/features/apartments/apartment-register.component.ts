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
  showModal = false;
  successMessage = '';

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
            code: '',
            door: `${i + 1}${this.getDoorLetter(i + 1)}`,
            floorId: floor.id,
            numRooms: this.numRooms && !isNaN(this.numRooms) ? Number(this.numRooms) : 1,
            numBathrooms: this.numBathrooms && !isNaN(this.numBathrooms) ? Number(this.numBathrooms) : 1,
            area: this.area && !isNaN(this.area) ? Number(this.area) : 70
          });
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
    if (!this.buildingId) {
      alert('Error: No se ha encontrado el ID del edificio');
      return;
    }

    if (this.isCreatingApartments) {
      return; // Evitar múltiples clics
    }

    // Si estamos en el paso 1, crear apartamentos automáticamente con características generales
    if (this.step === 1) {
      this.createApartmentsFromGeneralCharacteristics();
      return;
    }

    // Validar y limpiar datos antes de enviar (solo para el paso 2, que ya no se usa)
    const apartmentPayloads: any[] = [];
    const apartmentCreationRequests: any[] = [];
    for (const floorId in this.apartmentsByFloor) {
      for (const apt of this.apartmentsByFloor[floorId]) {
        // Limpiar y convertir todos los campos con validaciones del backend
        const numRooms = Number(apt.numRooms) || 1;
        const numBathrooms = Number(apt.numBathrooms) || 1;
        const area = Number(apt.area) || 1;

        const payload = {
          code: '', // El backend generará el código
          door: String(apt.door ?? '').trim(),
          floorId: String(apt.floorId ?? '').trim(),
          numRooms: Math.min(Math.max(1, numRooms), 50), // Limitar entre 1-50 (validación backend)
          numBathrooms: Math.min(Math.max(1, numBathrooms), 20), // Limitar entre 1-20 (validación backend)
          area: Math.max(0.01, area) // Mínimo 0.01 (validación backend)
        };

        // Validar campos obligatorios
        if (!payload.door || !payload.floorId) {
          alert('Por favor, completa todos los campos requeridos en todos los apartamentos.');
          return;
        }

        // Validar que la puerta no sea demasiado larga
        if (payload.door.length > 24) {
          alert(`La puerta "${payload.door}" es demasiado larga (máximo 24 caracteres). Corrige los valores antes de continuar.`);
          return;
        }

        // Advertir si se han ajustado valores extremos
        if (numRooms !== payload.numRooms || numBathrooms !== payload.numBathrooms) {
          console.warn(`Apartamento: Valores ajustados - Habitaciones: ${numRooms} → ${payload.numRooms}, Baños: ${numBathrooms} → ${payload.numBathrooms}`);
        }
        apartmentPayloads.push(payload);
        apartmentCreationRequests.push(this.api.createApartment(payload));
      }
    }

  // (No es necesario validar códigos duplicados, el backend los generará)

  // Log para depuración: mostrar los datos que se envían a la API
  console.log('Datos de apartamentos enviados a la API:', JSON.stringify(apartmentPayloads, null, 2));

    this.isCreatingApartments = true;

    // Ejecutar todas las creaciones en paralelo
    forkJoin(apartmentCreationRequests).subscribe({
      next: (results) => {
        console.log('Apartamentos creados exitosamente:', results);
        this.showSuccessModal(results.length);
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

  private createApartmentsFromGeneralCharacteristics() {
    if (this.apartmentsPerFloor < 1) this.apartmentsPerFloor = 1;

    // Validar que tenemos los datos básicos
    if (!this.buildingFloors.length) {
      alert('Error: No se han encontrado pisos para este edificio');
      return;
    }

    const apartmentCreationRequests: any[] = [];

    // Crear apartamentos para cada piso según las características generales
    this.buildingFloors.forEach(floor => {
      for (let i = 0; i < this.apartmentsPerFloor; i++) {
        const payload = {
          code: '', // El backend generará el código
          door: `${i + 1}${this.getDoorLetter(i + 1)}`,
          floorId: floor.id,
          numRooms: Math.min(Math.max(1, Number(this.numRooms) || 1), 50),
          numBathrooms: Math.min(Math.max(1, Number(this.numBathrooms) || 1), 20),
          area: Math.max(0.01, Number(this.area) || 70)
        };

        apartmentCreationRequests.push(this.api.createApartment(payload));
      }
    });

    console.log('Creando apartamentos con características generales...');
    this.isCreatingApartments = true;

    // Ejecutar todas las creaciones en paralelo
    forkJoin(apartmentCreationRequests).subscribe({
      next: (results) => {
        console.log('Apartamentos creados exitosamente:', results);
        this.showSuccessModal(results.length);
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
      // Generar nueva puerta
      const newIndex = apts.length;
  base.code = '';
  base.door = `${newIndex + 1}${this.getDoorLetter(newIndex + 1)}`;
  apts.push({ ...base });
    } else if (apts) {
      // Si no hay ninguno, crear uno básico
      apts.push({
        code: '',
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

  showSuccessModal(apartmentCount: number) {
    this.successMessage = `Se han registrado ${apartmentCount} apartamentos exitosamente`;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.router.navigate(['/buildings']); // Navegar de vuelta a la lista de edificios
  }
}
