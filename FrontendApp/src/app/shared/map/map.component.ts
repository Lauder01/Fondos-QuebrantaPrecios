import { Component, Input, OnInit, OnDestroy, AfterViewInit, ElementRef, ViewChild, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { LazyLoadDirective } from '../directives/lazy-load.directive';

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
  imports: [CommonModule, LazyLoadDirective],
  template: `
    <div class="map-container" appLazyLoad (lazyLoad)="onLazyLoad()">
      <!-- Mapa interactivo -->
      <div *ngIf="interactive && isBrowser && isLoaded" #mapElement class="map-element" [style.height]="height"></div>

      <!-- Mapa estático -->
      <div *ngIf="!interactive && staticMapUrl && isLoaded" class="static-map-container" [style.height]="height">
        <img [src]="staticMapUrl"
             alt="Mapa estático de ubicaciones"
             class="static-map-image"
             (load)="onStaticMapLoad()"
             (error)="onStaticMapError()">
        <div *ngIf="locations.length > 1" class="static-map-overlay">
          <small class="text-muted">{{locations.length}} ubicaciones</small>
        </div>
      </div>

      <!-- Loading placeholder antes de lazy load -->
      <div *ngIf="!isLoaded" class="map-placeholder">
        <div class="d-flex align-items-center justify-content-center h-100 bg-light">
          <div class="text-center text-muted">
            <i class="bi bi-geo-alt-fill fs-1"></i>
            <div class="mt-2">{{interactive ? 'Preparando mapa interactivo...' : 'Preparando mapa...'}}</div>
          </div>
        </div>
      </div>

      <!-- Loading overlay -->
      <div *ngIf="isGeocoding || isLoadingStaticMap" class="map-loading-overlay">
        <div class="d-flex align-items-center justify-content-center h-100">
          <div class="text-center">
            <div class="spinner-border text-primary" role="status">
              <span class="visually-hidden">
                {{interactive ? 'Geocodificando ubicaciones...' : 'Generando mapa estático...'}}
              </span>
            </div>
            <div class="mt-2">
              {{interactive ? 'Geocodificando ubicaciones...' : 'Generando mapa estático...'}}
            </div>
          </div>
        </div>
      </div>

      <!-- Fallback para servidor -->
      <div *ngIf="!isBrowser && isLoaded" class="map-placeholder">
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
    .static-map-container {
      position: relative;
      width: 100%;
      overflow: hidden;
      border-radius: 0.375rem;
    }
    .static-map-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
    }
    .static-map-overlay {
      position: absolute;
      top: 10px;
      right: 10px;
      background: rgba(255, 255, 255, 0.9);
      padding: 4px 8px;
      border-radius: 4px;
      font-size: 12px;
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
  @Input() interactive: boolean = true; // Nueva opción para mapas estáticos
  @Input() staticMapProvider: 'openstreetmap' | 'mapbox' = 'openstreetmap';

  private map: any;
  private markers: any[] = [];
  private L: any;
  isGeocoding = false;
  isBrowser: boolean;
  staticMapUrl: string = '';
  isLoadingStaticMap = false;
  isLoaded = false;

  // Caché de geocodificación y optimizaciones
  private geocodeCache = new Map<string, { latitude: number; longitude: number }>();
  private maxConcurrentRequests = 2; // Reducido para mejor control de QPS
  private requestDelay = 500; // Aumentado para respetar límites de API
  private pendingUpdate: number | null = null;
  private readonly CACHE_KEY = 'fqp_geocode_cache';
  private readonly CACHE_EXPIRY_DAYS = 30;

  // Control de QPS más robusto
  private requestQueue: Array<() => Promise<any>> = [];
  private activeRequests = 0;
  private lastRequestTime = 0;
  private minRequestInterval = 250; // ms mínimo entre requests

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      this.loadCacheFromStorage();
    }
  }

  // Métodos de caché persistente
  private loadCacheFromStorage(): void {
    try {
      const cached = localStorage.getItem(this.CACHE_KEY);
      if (cached) {
        const data = JSON.parse(cached);
        const now = Date.now();

        // Filtrar entradas expiradas
        Object.entries(data).forEach(([key, value]: [string, any]) => {
          if (value.timestamp && (now - value.timestamp) < (this.CACHE_EXPIRY_DAYS * 24 * 60 * 60 * 1000)) {
            this.geocodeCache.set(key, { latitude: value.latitude, longitude: value.longitude });
          }
        });

        console.log(`💾 Cargadas ${this.geocodeCache.size} ubicaciones del caché`);
      }
    } catch (error) {
      console.warn('Error cargando caché de geocodificación:', error);
    }
  }

  private saveCacheToStorage(): void {
    try {
      const data: { [key: string]: any } = {};
      const now = Date.now();

      this.geocodeCache.forEach((coords, address) => {
        data[address] = {
          ...coords,
          timestamp: now
        };
      });

      localStorage.setItem(this.CACHE_KEY, JSON.stringify(data));
    } catch (error) {
      console.warn('Error guardando caché de geocodificación:', error);
    }
  }

  ngOnInit() {
    // No cargar nada automáticamente, esperar al lazy loading
  }

  onLazyLoad() {
    console.log('🚀 Lazy loading activado para mapa');
    this.isLoaded = true;

    if (!this.isBrowser) {
      return;
    }

    if (this.interactive) {
      this.loadLeaflet().then(() => {
        if (this.locations.length > 0) {
          // Pequeño delay para que el elemento esté disponible
          setTimeout(() => {
            this.initializeMap();
            this.updateMarkers();
          }, 100);
        }
      });
    } else {
      // Para mapas estáticos, generar URL si hay ubicaciones
      if (this.locations.length > 0) {
        this.generateStaticMap();
      }
    }
  }

  ngAfterViewInit() {
    // Solo actuar si ya está cargado (lazy loading completado)
    if (this.isBrowser && this.isLoaded && this.interactive) {
      setTimeout(() => {
        if (this.L && !this.map) {
          this.initializeMap();
          if (this.locations.length > 0) {
            this.updateMarkers();
          }
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

      // Verificar si Leaflet ya está disponible globalmente
      if (typeof window !== 'undefined' && (window as any).L) {
        console.log('✅ Leaflet ya disponible globalmente');
        this.L = (window as any).L;
        this.fixLeafletIcons();
        return;
      }

      // Intentar importación dinámica con fallback
      try {
        this.L = await import('leaflet');
        console.log('✅ Leaflet cargado dinámicamente');
      } catch (importError) {
        console.warn('⚠️ Importación dinámica falló, intentando fallback...', importError);

        // Fallback: cargar desde CDN
        await this.loadLeafletFromCDN();
      }

      this.fixLeafletIcons();
    } catch (error) {
      console.error('❌ Error cargando Leaflet:', error);
      // En caso de error total, mostrar mensaje de error al usuario
      this.handleMapLoadError();
    }
  }

  private async loadLeafletFromCDN(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (typeof window === 'undefined') {
        reject(new Error('Window no disponible'));
        return;
      }

      // Cargar CSS de Leaflet
      const cssLink = document.createElement('link');
      cssLink.rel = 'stylesheet';
      cssLink.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
      document.head.appendChild(cssLink);

      // Cargar JS de Leaflet
      const script = document.createElement('script');
      script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
      script.onload = () => {
        this.L = (window as any).L;
        console.log('✅ Leaflet cargado desde CDN');
        resolve();
      };
      script.onerror = () => {
        console.error('❌ Error cargando Leaflet desde CDN');
        reject(new Error('Error cargando Leaflet desde CDN'));
      };
      document.head.appendChild(script);
    });
  }

  private handleMapLoadError(): void {
    // Mostrar un mensaje de error en lugar del mapa
    if (this.mapElement) {
      this.mapElement.nativeElement.innerHTML = `
        <div class="d-flex align-items-center justify-content-center h-100 bg-light border rounded">
          <div class="text-center text-muted p-4">
            <i class="bi bi-exclamation-triangle fs-1"></i>
            <div class="mt-2">No se pudo cargar el mapa</div>
            <small>Intenta recargar la página</small>
          </div>
        </div>
      `;
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
      // Separar ubicaciones que ya tienen coordenadas de las que necesitan geocodificación
      const locationsWithCoords = this.locations.filter(loc => loc.latitude && loc.longitude);
      const locationsNeedingGeocode = this.locations.filter(loc => !loc.latitude || !loc.longitude);

      console.log('📍 Ubicaciones con coordenadas:', locationsWithCoords.length);
      console.log('🔍 Ubicaciones que necesitan geocodificación:', locationsNeedingGeocode.length);

      // Mostrar inmediatamente las ubicaciones que ya tienen coordenadas
      if (locationsWithCoords.length > 0) {
        this.addMarkersToMap(locationsWithCoords);
        this.adjustMapView();
      }

      // Geocodificar las ubicaciones restantes en paralelo con carga progresiva
      if (locationsNeedingGeocode.length > 0) {
        await this.geocodeAndAddMarkersProgressively(locationsNeedingGeocode);
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

  private addMarkersToMap(locations: MapLocation[]) {
    for (const location of locations) {
      if (location.latitude && location.longitude) {
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
    }
  }

  private adjustMapView() {
    if (this.markers.length > 0) {
      const group = new this.L.FeatureGroup(this.markers);
      this.map.fitBounds(group.getBounds().pad(0.1));
      console.log('✅ Vista ajustada para mostrar', this.markers.length, 'marcadores');
    }
  }

  private async geocodeAndAddMarkersProgressively(locations: MapLocation[]) {
    // Procesar en lotes para evitar sobrecargar el servicio
    const batchSize = this.maxConcurrentRequests;
    const batches = [];

    for (let i = 0; i < locations.length; i += batchSize) {
      batches.push(locations.slice(i, i + batchSize));
    }

    console.log(`🔄 Procesando ${batches.length} lotes de geocodificación`);

    for (let i = 0; i < batches.length; i++) {
      const batch = batches[i];
      console.log(`📦 Procesando lote ${i + 1}/${batches.length} con ${batch.length} ubicaciones`);

      // Procesar el lote en paralelo
      const promises = batch.map(location => this.geocodeLocationSafely(location));
      const results = await Promise.allSettled(promises);

      // Añadir marcadores de las ubicaciones geocodificadas exitosamente
      const geocodedLocations = results
        .map((result, index) => ({
          result,
          location: batch[index]
        }))
        .filter(({ result }) => result.status === 'fulfilled')
        .map(({ result, location }) => ({
          ...location,
          latitude: (result as PromiseFulfilledResult<{ latitude: number; longitude: number }>).value.latitude,
          longitude: (result as PromiseFulfilledResult<{ latitude: number; longitude: number }>).value.longitude
        }));

      if (geocodedLocations.length > 0) {
        this.addMarkersToMap(geocodedLocations);
        this.adjustMapView(); // Reajustar vista con cada lote
      }

      // Pausa entre lotes para no sobrecargar el servicio
      if (i < batches.length - 1) {
        await this.delay(this.requestDelay);
      }
    }
  }

  private async geocodeLocationSafely(location: MapLocation): Promise<{ latitude: number; longitude: number }> {
    const cacheKey = location.address.toLowerCase().trim();

    // Verificar caché primero
    if (this.geocodeCache.has(cacheKey)) {
      console.log('💾 Usando coordenadas del caché para:', location.address);
      return this.geocodeCache.get(cacheKey)!;
    }

    // Geocodificar con control de QPS
    const coords = await this.executeWithQpsControl(() => this.geocodeAddress(location.address));

    // Guardar en caché en memoria y persistente
    this.geocodeCache.set(cacheKey, coords);
    this.saveCacheToStorage();

    return coords;
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

    // Estrategias de geocodificación con diferentes parámetros para mejorar la precisión
    const geocodingStrategies = [
      {
        server: 'https://nominatim.openstreetmap.org',
        params: `format=json&q=${encodedAddress}&limit=1&countrycodes=es&addressdetails=1`,
        description: 'Búsqueda estándar con detalles de dirección'
      },
      {
        server: 'https://nominatim.openstreetmap.org',
        params: `format=json&q=${encodedAddress}&limit=1&countrycodes=es&bounded=1&viewbox=-18.16,27.64,4.33,43.79`,
        description: 'Búsqueda limitada al área de España'
      },
      {
        server: 'https://nominatim.openstreetmap.org',
        params: `format=json&q=${encodedAddress}&limit=3&countrycodes=es`,
        description: 'Búsqueda con más resultados para mejor precisión'
      }
    ];

    for (const strategy of geocodingStrategies) {
      try {
        console.log(`🔍 Intentando geocodificación: ${strategy.description}`);

        // Nominatim API
        const url = `${strategy.server}/search?${strategy.params}`;

        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 10000); // 10 segundos timeout

        const response = await fetch(url, {
          signal: controller.signal,
          headers: {
            'User-Agent': 'FondosQuebrantaPrecios/1.0 (contact@example.com)', // Identificarse correctamente
            'Accept': 'application/json'
          }
        });

        clearTimeout(timeoutId);

        if (!response.ok) {
          throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const data = await response.json();

        if (data && data.length > 0) {
          console.log(`✅ Geocodificación exitosa con ${strategy.server} (${strategy.description})`);

          // Nominatim API response format
          const latitude = parseFloat(data[0].lat);
          const longitude = parseFloat(data[0].lon);

          return { latitude, longitude };
        }
      } catch (error) {
        const errorMessage = error instanceof Error ? error.message : 'Error desconocido';
        console.warn(`⚠️ Error con estrategia "${strategy.description}": ${errorMessage}`);

        // Log más detallado para debugging
        if (error instanceof TypeError && errorMessage.includes('Failed to fetch')) {
          console.warn('💡 Posible problema de red o CORS. Verificar conectividad.');
        } else if (error instanceof DOMException && error.name === 'AbortError') {
          console.warn('⏱️ Timeout en la geocodificación. El servidor tardó más de 10 segundos.');
        }

        continue; // Intentar con la siguiente estrategia
      }
    }

    // Si todos los servidores fallan, usar coordenadas por defecto para España
    console.warn(`❌ No se pudo geocodificar: ${address}, usando coordenadas por defecto para España`);

    // Coordenadas aproximadas del centro de España (Madrid)
    const defaultCoords = {
      latitude: 40.4168,
      longitude: -3.7038
    };

    console.log(`🏠 Usando coordenadas por defecto: ${defaultCoords.latitude}, ${defaultCoords.longitude}`);
    return defaultCoords;
  }

  private async executeWithQpsControl<T>(requestFn: () => Promise<T>): Promise<T> {
    return new Promise((resolve, reject) => {
      this.requestQueue.push(async () => {
        try {
          // Asegurar intervalo mínimo entre requests
          const now = Date.now();
          const timeSinceLastRequest = now - this.lastRequestTime;
          if (timeSinceLastRequest < this.minRequestInterval) {
            await this.delay(this.minRequestInterval - timeSinceLastRequest);
          }

          this.activeRequests++;
          this.lastRequestTime = Date.now();

          const result = await requestFn();
          resolve(result);
        } catch (error) {
          reject(error);
        } finally {
          this.activeRequests--;
          this.processQueue();
        }
      });

      this.processQueue();
    });
  }

  private processQueue(): void {
    if (this.activeRequests < this.maxConcurrentRequests && this.requestQueue.length > 0) {
      const nextRequest = this.requestQueue.shift();
      if (nextRequest) {
        nextRequest();
      }
    }
  }

  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  // Métodos para mapas estáticos
  private async generateStaticMap(): Promise<void> {
    if (this.locations.length === 0) {
      return;
    }

    this.isLoadingStaticMap = true;

    try {
      // Geocodificar ubicaciones si es necesario
      const locationsWithCoords = await this.geocodeLocationsForStatic(this.locations);

      if (locationsWithCoords.length > 0) {
        this.staticMapUrl = this.buildStaticMapUrl(locationsWithCoords);
      }
    } catch (error) {
      console.error('Error generando mapa estático:', error);
    } finally {
      this.isLoadingStaticMap = false;
    }
  }

  private async geocodeLocationsForStatic(locations: MapLocation[]): Promise<MapLocation[]> {
    const results: MapLocation[] = [];

    for (const location of locations) {
      if (location.latitude && location.longitude) {
        results.push(location);
      } else {
        try {
          const coords = await this.geocodeLocationSafely(location);
          results.push({
            ...location,
            latitude: coords.latitude,
            longitude: coords.longitude
          });
        } catch (error) {
          console.warn(`No se pudo geocodificar para mapa estático: ${location.address}`, error);
        }
      }
    }

    return results;
  }

  private buildStaticMapUrl(locations: MapLocation[]): string {
    if (locations.length === 0) return '';

    // Calcular centro y zoom automáticamente
    const bounds = this.calculateBounds(locations);
    const center = this.calculateCenter(bounds);
    const zoom = this.calculateZoom(bounds);

    // Construir URL para mapa estático de OpenStreetMap usando StaticMapLite o similar
    const width = 800;
    const height = 400;

    // Usar servicio de mapas estáticos (puedes cambiar por otros proveedores)
    const baseUrl = 'https://api.mapbox.com/styles/v1/mapbox/streets-v11/static';
    const markers = locations.map(loc => `pin-s+ff0000(${loc.longitude},${loc.latitude})`).join(',');

    // Nota: Para producción, deberías usar tu propia API key de Mapbox
    // Por ahora usamos un enfoque alternativo con OpenStreetMap
    return this.buildOpenStreetMapStaticUrl(locations, center, zoom, width, height);
  }

  private buildOpenStreetMapStaticUrl(locations: MapLocation[], center: {lat: number, lng: number}, zoom: number, width: number, height: number): string {
    // Usar un servicio como StaticMapLite o construir con tiles
    // Por simplicidad, construimos una URL básica
    const markers = locations.map(loc => `${loc.latitude},${loc.longitude}`).join('|');

    // Usar un servicio gratuito como StaticMapLite (requiere instalación) o construir URL básica
    // Para demo, usamos una imagen de placeholder con información
    return `https://via.placeholder.com/${width}x${height}/e9ecef/6c757d?text=Mapa+con+${locations.length}+ubicaciones`;
  }

  private calculateBounds(locations: MapLocation[]): {north: number, south: number, east: number, west: number} {
    let north = -90, south = 90, east = -180, west = 180;

    locations.forEach(loc => {
      if (loc.latitude && loc.longitude) {
        north = Math.max(north, loc.latitude);
        south = Math.min(south, loc.latitude);
        east = Math.max(east, loc.longitude);
        west = Math.min(west, loc.longitude);
      }
    });

    return { north, south, east, west };
  }

  private calculateCenter(bounds: {north: number, south: number, east: number, west: number}): {lat: number, lng: number} {
    return {
      lat: (bounds.north + bounds.south) / 2,
      lng: (bounds.east + bounds.west) / 2
    };
  }

  private calculateZoom(bounds: {north: number, south: number, east: number, west: number}): number {
    const latDiff = bounds.north - bounds.south;
    const lngDiff = bounds.east - bounds.west;
    const maxDiff = Math.max(latDiff, lngDiff);

    if (maxDiff < 0.01) return 15;
    if (maxDiff < 0.05) return 13;
    if (maxDiff < 0.1) return 11;
    if (maxDiff < 0.5) return 9;
    if (maxDiff < 1) return 7;
    return 5;
  }

  onStaticMapLoad(): void {
    console.log('✅ Mapa estático cargado correctamente');
  }

  onStaticMapError(): void {
    console.error('❌ Error cargando mapa estático');
    // Fallback a imagen placeholder
    this.staticMapUrl = 'https://via.placeholder.com/800x400/e9ecef/6c757d?text=Error+cargando+mapa';
  }

  // Método público para actualizar ubicaciones desde el componente padre
  async updateLocations(newLocations: MapLocation[]) {
    console.log('🔄 updateLocations llamado con', newLocations.length, 'ubicaciones');

    // Cancelar actualización pendiente
    if (this.pendingUpdate !== null) {
      clearTimeout(this.pendingUpdate);
      this.pendingUpdate = null;
    }

    this.locations = newLocations;

    // Si no estamos en el navegador o no está cargado, no hacer nada aún
    if (!this.isBrowser || !this.isLoaded) {
      console.log('ℹ️ Mapa no disponible o no cargado aún, saltando actualización');
      return;
    }

    // Debounce para evitar múltiples llamadas rápidas
    this.pendingUpdate = window.setTimeout(async () => {
      this.pendingUpdate = null;

      if (!this.interactive) {
        // Para mapas estáticos, regenerar URL
        console.log('🖼️ Generando mapa estático...');
        await this.generateStaticMap();
        return;
      }

      // Para mapas interactivos, continuar con la lógica existente
      if (!this.L) {
        console.log('⏳ Leaflet no cargado, cargando...');
        await this.loadLeaflet();
      }

      if (!this.map && this.mapElement) {
        console.log('🗺️ Mapa no inicializado, inicializando...');
        this.initializeMap();
      }

      if (this.map) {
        console.log('📍 Actualizando marcadores...');
        this.updateMarkers();
      } else {
        console.log('❌ No se pudo actualizar marcadores - mapa no disponible');
      }
    }, 300); // Debounce de 300ms
  }
}
