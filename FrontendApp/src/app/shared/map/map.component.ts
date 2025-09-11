import { Component, Input, OnInit, OnDestroy, AfterViewInit, ElementRef, ViewChild, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';

export interface MapLocation {
  id: string;
  name: string;
  address: string;
  latitude?: number;
  longitude?: number;
}

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="map-container">
      <div #mapElement class="map-element" [style.height]="height"></div>
      <div *ngIf="isGeocoding" class="map-loading-overlay">
        <div class="d-flex align-items-center justify-content-center h-100">
          <div class="text-center">
            <div class="spinner-border text-primary" role="status">
              <span class="visually-hidden">Geocodificando ubicaciones...</span>
            </div>
            <div class="mt-2">Geocodificando ubicaciones...</div>
          </div>
        </div>
      </div>
      <div *ngIf="!isBrowser" class="map-placeholder">
        <div class="d-flex align-items-center justify-content-center h-100 bg-light">
          <div class="text-center text-muted">
            <i class="bi bi-geo-alt-fill fs-1"></i>
            <div class="mt-2">Mapa no disponible en servidor</div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .map-container {
      position: relative;
      width: 100%;
    }
    .map-element {
      width: 100%;
      z-index: 1;
    }
    .map-loading-overlay, .map-placeholder {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background-color: rgba(255, 255, 255, 0.8);
      z-index: 1000;
    }
    .map-placeholder {
      background-color: #f8f9fa;
      border: 1px solid #dee2e6;
      border-radius: 0.375rem;
    }
  `]
})
export class MapComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('mapElement', { static: false }) mapElement!: ElementRef;
  @Input() locations: MapLocation[] = [];
  @Input() height: string = '400px';
  @Input() centerLat: number = 40.4168; // Madrid por defecto
  @Input() centerLng: number = -3.7038;
  @Input() zoom: number = 10;

  private map: any;
  private markers: any[] = [];
  private L: any;
  isGeocoding = false;
  isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit() {
    // Solo cargar Leaflet en el navegador
    if (this.isBrowser) {
      this.loadLeaflet();
    }
  }

  ngAfterViewInit() {
    if (this.isBrowser) {
      // Pequeño delay para asegurar que el elemento está renderizado
      setTimeout(() => {
        if (this.L) {
          this.initializeMap();
          if (this.locations.length > 0) {
            this.updateMarkers();
          }
        } else {
          // Si Leaflet no está cargado, lo cargamos y luego inicializamos
          this.loadLeaflet().then(() => {
            this.initializeMap();
            if (this.locations.length > 0) {
              this.updateMarkers();
            }
          });
        }
      }, 100);
    }
  }

  ngOnDestroy() {
    if (this.map) {
      this.map.remove();
    }
  }

  private async loadLeaflet() {
    try {
      console.log('🗺️ Cargando Leaflet...');
      // Importación dinámica de Leaflet solo en el navegador
      this.L = await import('leaflet');
      console.log('✅ Leaflet cargado correctamente');
      this.fixLeafletIcons();
    } catch (error) {
      console.error('❌ Error cargando Leaflet:', error);
    }
  }

  private fixLeafletIcons() {
    if (!this.L) return;

    // Fix para que los iconos de Leaflet funcionen correctamente
    delete (this.L.Icon.Default.prototype as any)._getIconUrl;
    this.L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
      iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
      shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
    });
  }

  private initializeMap() {
    if (!this.mapElement || !this.L) {
      console.log('❌ No se puede inicializar mapa:', { mapElement: !!this.mapElement, L: !!this.L });
      return;
    }

    console.log('🗺️ Inicializando mapa...');
    this.map = this.L.map(this.mapElement.nativeElement).setView([this.centerLat, this.centerLng], this.zoom);

    // Añadir tiles de OpenStreetMap
    this.L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this.map);

    console.log('✅ Mapa inicializado correctamente');
  }

  async updateMarkers() {
    if (!this.map || !this.L) {
      console.log('❌ No se pueden actualizar marcadores:', { map: !!this.map, L: !!this.L });
      return;
    }

    console.log('🎯 Actualizando marcadores para', this.locations.length, 'ubicaciones');

    // Limpiar marcadores existentes
    this.clearMarkers();

    if (this.locations.length === 0) {
      console.log('ℹ️ No hay ubicaciones para mostrar');
      return;
    }

    this.isGeocoding = true;

    try {
      // Geocodificar ubicaciones que no tienen coordenadas
      const geocodedLocations = await this.geocodeLocations(this.locations);

      // Añadir marcadores
      const validLocations = geocodedLocations.filter(loc => loc.latitude && loc.longitude);
      console.log('📍 Ubicaciones válidas encontradas:', validLocations.length);

      for (const location of validLocations) {
        console.log('📌 Añadiendo marcador para:', location.name, [location.latitude, location.longitude]);
        const marker = this.L.marker([location.latitude!, location.longitude!])
          .addTo(this.map)
          .bindPopup(`
            <div>
              <strong>${location.name}</strong><br>
              ${location.address}
            </div>
          `);

        this.markers.push(marker);
      }

      // Ajustar la vista para mostrar todos los marcadores
      if (validLocations.length > 0) {
        const group = new this.L.FeatureGroup(this.markers);
        this.map.fitBounds(group.getBounds().pad(0.1));
        console.log('✅ Vista ajustada para mostrar todos los marcadores');
      }
    } catch (error) {
      console.error('❌ Error geocodificando ubicaciones:', error);
    } finally {
      this.isGeocoding = false;
    }
  }

  private clearMarkers() {
    this.markers.forEach(marker => marker.remove());
    this.markers = [];
  }

  private async geocodeLocations(locations: MapLocation[]): Promise<MapLocation[]> {
    const geocodedLocations: MapLocation[] = [];

    for (const location of locations) {
      try {
        if (location.latitude && location.longitude) {
          // Ya tiene coordenadas
          geocodedLocations.push(location);
        } else {
          // Necesita geocodificación
          const coords = await this.geocodeAddress(location.address);
          geocodedLocations.push({
            ...location,
            latitude: coords.latitude,
            longitude: coords.longitude
          });
        }

        // Pequeña pausa para no sobrecargar el servicio de geocodificación
        await this.delay(100);
      } catch (error) {
        console.warn(`No se pudo geocodificar: ${location.address}`, error);
        // Añadir sin coordenadas (no se mostrará en el mapa)
        geocodedLocations.push(location);
      }
    }

    return geocodedLocations;
  }

  private async geocodeAddress(address: string): Promise<{ latitude: number; longitude: number }> {
    // Usar Nominatim (OpenStreetMap) para geocodificación gratuita
    const encodedAddress = encodeURIComponent(address);
    const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodedAddress}&limit=1`;

    const response = await fetch(url);
    const data = await response.json();

    if (data && data.length > 0) {
      return {
        latitude: parseFloat(data[0].lat),
        longitude: parseFloat(data[0].lon)
      };
    } else {
      throw new Error('No se encontraron coordenadas para la dirección');
    }
  }

  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  // Método público para actualizar ubicaciones desde el componente padre
  async updateLocations(newLocations: MapLocation[]) {
    console.log('🔄 updateLocations llamado con', newLocations.length, 'ubicaciones');
    this.locations = newLocations;

    // Si no estamos en el navegador, no hacer nada
    if (!this.isBrowser) {
      console.log('ℹ️ No estamos en el navegador, saltando actualización');
      return;
    }

    // Si Leaflet no está cargado aún, esperamos
    if (!this.L) {
      console.log('⏳ Leaflet no cargado, cargando...');
      await this.loadLeaflet();
    }

    // Si el mapa no está inicializado, lo inicializamos
    if (!this.map && this.mapElement) {
      console.log('🗺️ Mapa no inicializado, inicializando...');
      this.initializeMap();
    }

    // Actualizar marcadores
    if (this.map) {
      console.log('📍 Actualizando marcadores...');
      this.updateMarkers();
    } else {
      console.log('❌ No se pudo actualizar marcadores - mapa no disponible');
    }
  }
}
