import { Component, OnInit, OnDestroy, NgZone, ChangeDetectorRef, ChangeDetectionStrategy, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuildingCardComponent } from '../building-card/building-card.component';
import { MapComponent, MapLocation } from '../shared/map/map.component';
import { BuildingService } from './building.service';
import { ApiService, DistrictGetterDto, StatusGetterDto } from '../core/api.service';
import { forkJoin, of, Subject } from 'rxjs';
import { timeout, catchError, take, takeUntil } from 'rxjs/operators';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-building-list',
  standalone: true,
  imports: [CommonModule, FormsModule, BuildingCardComponent, MapComponent],
  templateUrl: './building-list.component.html',
  styleUrls: ['./building-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BuildingListComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MapComponent) mapComponent!: MapComponent;

  private destroy$ = new Subject<void>();
  loading = false;
  buildings: any[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 6;
  searchName = '';

  // Propiedades del mapa
  mapLocations: MapLocation[] = [];
  showMap = true;

  districts: DistrictGetterDto[] = [];
  statuses: StatusGetterDto[] = [];
  districtMap: { [id: string]: string } = {};
  statusMap: { [id: string]: string } = {};

  constructor(
    private buildingService: BuildingService,
    private apiService: ApiService,
    private zone: NgZone,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) {
    // Escuchar cambios de navegación para recargar cuando se navega a esta ruta
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      takeUntil(this.destroy$)
    ).subscribe((event: NavigationEnd) => {
      if (event.url === '/buildings' || event.url.startsWith('/buildings?')) {
        console.log('Navegación detectada a buildings, recargando datos...');
        setTimeout(() => {
          this.resetAndLoadData();
        }, 100);
      }
    });
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  getPagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  ngOnInit(): void {
    this.resetAndLoadData();
  }

  ngAfterViewInit(): void {
    // Después de que la vista esté inicializada, intentar actualizar el mapa si ya hay datos
    // El debounce del mapa evitará llamadas duplicadas, así que podemos llamar directamente
    if (this.buildings.length > 0 && this.mapLocations.length > 0) {
      this.updateMapLocations();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private resetAndLoadData(): void {
    console.log('Reseteando y cargando datos...');
    // Reset del estado
    this.buildings = [];
    this.totalCount = 0;
    this.page = 1;
    this.loading = false;
    this.districts = [];
    this.statuses = [];
    this.districtMap = {};
    this.statusMap = {};

    // Forzar detección de cambios
    this.cdr.detectChanges();

    // Cargar datos
    this.loadCatalogsAndBuildings();
  }

  /** 1) Garantiza que catálogos COMPLETAN (take(1)); 2) Dentro de NgZone */
  private loadCatalogsAndBuildings(): void {
    if (this.loading) {
      console.log('Ya está cargando, saltando...');
      return;
    }

    console.log('Iniciando carga de catálogos y edificios...');
    forkJoin({
      districts: this.apiService.getDistricts().pipe(
        timeout(10000),
        catchError(() => of([])),
        take(1),
        takeUntil(this.destroy$)
      ),
      statuses: this.apiService.getStatuses().pipe(
        timeout(10000),
        catchError(() => of([])),
        take(1),
        takeUntil(this.destroy$)
      )
    }).subscribe({
      next: ({ districts, statuses }) => {
        this.zone.run(() => {
          console.log('Catálogos cargados:', { districts: districts?.length, statuses: statuses?.length });
          console.log('Status recibidos:', statuses);
          this.districts = districts || [];
          this.statuses = statuses || [];
          this.districtMap = {};
          this.statusMap = {};
          this.districts.forEach(d => { if (d?.id) this.districtMap[d.id] = d.name || '-'; });
          this.statuses.forEach(s => { if (s?.id) this.statusMap[s.id] = s.name || '-'; });

          console.log('Status map creado:', this.statusMap);

          // catálogos listos -> ahora edificios
          this.fetchBuildings();
          this.cdr.detectChanges();
        });
      },
      error: (error) => {
        console.error('Error loading catalogs:', error);
        // incluso si falla, cargamos edificios (mostrarán '-')
        this.fetchBuildings();
      }
    });
  }

  /** También dentro de NgZone para forzar CD si la API usa fetch/no-zone */
  fetchBuildings(): void {
    if (this.loading) {
      console.log('Ya está cargando edificios, saltando...');
      return;
    }

    console.log('Iniciando carga de edificios...', { page: this.page, pageSize: this.pageSize, searchName: this.searchName });
    this.loading = true;
    this.cdr.detectChanges(); // Forzar actualización del loading

    this.buildingService.getBuildings(this.page, this.pageSize, this.searchName)
      .pipe(
        timeout(10000),
        catchError(error => {
          console.error('Error fetching buildings:', error);
          return of({ items: [], totalCount: 0, page: this.page, pageSize: this.pageSize });
        }),
        take(1),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (result) => {
          console.log('RESULTADO API BUILDINGS:', result);
          this.zone.run(() => {
            this.buildings = (result.items || []).map(b => {
              console.log('Procesando edificio:', b.name, 'StatusID:', b.statusId, 'StatusName en map:', this.statusMap[b?.statusId || '']);
              return {
                ...b,
                name: (b?.name && b.name.trim()) ? b.name : (b?.constructedAddress && b.constructedAddress.trim() ? b.constructedAddress : 'Edificio sin dirección'),
                districtName: this.districtMap[b?.districtId || ''] || '-',
                statusName: this.statusMap[b?.statusId || ''] || 'Sin estado'
              };
            });
            this.totalCount = result.totalCount || 0;
            this.loading = false;

            console.log('Edificios procesados:', this.buildings.length);
            console.log('Total count:', this.totalCount);
            console.log('Primer edificio con status:', this.buildings[0]);

            // Actualizar ubicaciones del mapa
            this.updateMapLocations();

            // Forzar detección de cambios
            this.cdr.detectChanges();
          });
        },
        error: (error) => {
          console.error('Unexpected error:', error);
          this.zone.run(() => {
            this.buildings = [];
            this.totalCount = 0;
            this.loading = false;
            this.cdr.detectChanges();
          });
        }
      });
  }

  goToPage(newPage: number): void {
    if (newPage < 1 || newPage > this.totalPages || this.loading || newPage === this.page) return;
    console.log('Cambiando a página:', newPage);
    this.page = newPage;
    this.fetchBuildings();
  }

  onPageSizeChange(): void {
    if (this.loading) return;
    console.log('Cambiando tamaño de página a:', this.pageSize);
    this.page = 1;
    this.fetchBuildings();
  }

  onSearchChange(): void {
    if (this.loading) return;
    console.log('Búsqueda cambiada a:', this.searchName);
    this.page = 1;
    this.fetchBuildings();
  }

  onCreateBuilding(): void {
    // Implementar navegación a formulario de creación si es necesario
  }

  // TrackBy function para optimizar el rendering del *ngFor
  trackByBuildingId(index: number, building: any): string {
    return building?.id || index.toString();
  }

  // Método para actualizar las ubicaciones del mapa
  private updateMapLocations(): void {
    console.log('🏢 Actualizando ubicaciones del mapa con', this.buildings.length, 'edificios');

    this.mapLocations = this.buildings.map(building => {
      const location = {
        id: building.id,
        name: building.name || 'Edificio sin nombre',
        address: this.buildFullAddress(building)
      };
      console.log('🏢 Procesando edificio:', location);
      return location;
    });

    console.log('📋 Ubicaciones del mapa creadas:', this.mapLocations.length);

    // Actualizar el mapa si está disponible (el debounce interno del mapa evitará llamadas duplicadas)
    if (this.mapComponent) {
      console.log('🗺️ Actualizando componente de mapa...');
      this.mapComponent.updateLocations(this.mapLocations);
    } else {
      console.log('⚠️ Componente de mapa no disponible aún');
    }
  }

  // Método para actualizar las ubicaciones del mapa con delay (solo si es necesario)
  private updateMapLocationsDelayed(): void {
    // Solo actualizar si no se ha actualizado recientemente y hay datos nuevos
    if (this.mapComponent && this.mapLocations.length > 0) {
      console.log('🕐 Actualización tardía del mapa...');
      this.mapComponent.updateLocations(this.mapLocations);
    }
  }

  // Método para construir la dirección completa
  private buildFullAddress(building: any): string {
    let address = '';

    if (building.constructedAddress) {
      address = building.constructedAddress;
    } else {
      // Construir dirección a partir de los componentes disponibles
      const parts = [];
      if (building.doorway) parts.push(building.doorway);
      if (building.districtName) parts.push(building.districtName);
      if (building.city) parts.push(building.city);
      if (building.country) parts.push(building.country);
      address = parts.join(', ');
    }

    return address || 'Dirección no disponible';
  }

  // Método para alternar la visibilidad del mapa
  toggleMap(): void {
    this.showMap = !this.showMap;
  }
}
