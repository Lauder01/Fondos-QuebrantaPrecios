import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ImageService, BuildingImageDto } from '../../../core/image.service';

export interface BuildingImage {
  buildingImageId?: string; // Opcional para nuevas imágenes
  url: string;
  fileName: string;
  altText: string;
  isCover: boolean;
  uploading?: boolean;
  error?: string;
  imageError?: boolean; // Error al cargar la imagen
  size?: number;
  file?: File; // Archivo temporal antes de subir al servidor
  blobUrl?: string; // URL blob temporal para limpieza
}

@Component({
  selector: 'app-building-images',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './building-images.component.html',
  styleUrls: ['./building-images.component.css']
})
export class BuildingImagesComponent implements OnInit, OnDestroy {
  @Input() images: BuildingImage[] = [];
  @Input() buildingId: string = ''; // ID del edificio para cargar/guardar imágenes
  @Output() imagesChange = new EventEmitter<BuildingImage[]>();

  uploading = false;

  constructor(private imageService: ImageService) { }

  ngOnInit() {
    if (this.buildingId) {
      this.loadBuildingImages();
    }
  }

  /**
   * Carga las imágenes existentes del edificio
   */
  loadBuildingImages() {
    console.log('Iniciando carga de imágenes para buildingId:', this.buildingId);

    this.imageService.getBuildingImages(this.buildingId).subscribe({
      next: (buildingImages) => {
        console.log('Respuesta de la API - Total imágenes:', buildingImages.length, buildingImages);

        this.images = buildingImages.map(img => {
          const imageUrl = this.imageService.getImageUrl(img.buildingImageId);
          console.log('Procesando imagen:', {
            fileName: img.fileName,
            buildingImageId: img.buildingImageId,
            generatedUrl: imageUrl,
            hasImageData: img.hasImageData,
            size: img.size,
            downloadUrl: img.downloadUrl
          });

          return {
            buildingImageId: img.buildingImageId,
            url: imageUrl,
            fileName: img.fileName,
            altText: img.altText,
            isCover: img.isCoverImage,
            size: img.size,
            imageError: false
          };
        });

        console.log('Imágenes procesadas:', this.images);
        this.imagesChange.emit(this.images);

        // En producción, intentar precargar las imágenes
        if (this.isProduction()) {
          this.preloadImagesForProduction();
        }
      },
      error: (error) => {
        console.error('Error completo cargando imágenes:', {
          error: error,
          status: error.status,
          statusText: error.statusText,
          url: error.url,
          message: error.message
        });
      }
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;
    const files = Array.from(input.files);
    files.forEach(file => this.uploadImage(file));
  }

  uploadImage(file: File) {
    // Crear URL temporal para mostrar la imagen
    const objectUrl = URL.createObjectURL(file);

    // Crear imagen temporal para mostrar
    const tempImage: BuildingImage = {
      url: objectUrl,
      blobUrl: objectUrl, // Guardar para limpieza posterior
      fileName: file.name,
      altText: '',
      isCover: this.images.length === 0, // Primera imagen es portada por defecto
      uploading: false,
      file: file // Guardar archivo para subir después
    };
    this.images.push(tempImage);
    this.imagesChange.emit(this.images);
  }

  setAsCover(index: number) {
    const image = this.images[index];
    if (!image.buildingImageId) {
      console.error('No se puede establecer como portada: imagen no guardada');
      return;
    }

    this.imageService.setAsCoverImage(image.buildingImageId).subscribe({
      next: () => {
        // Actualizar estado local
        this.images.forEach((img, i) => img.isCover = i === index);
        this.imagesChange.emit(this.images);
      },
      error: (error) => {
        console.error('Error estableciendo como portada:', error);
      }
    });
  }

  removeImage(index: number) {
    const image = this.images[index];

    // Limpiar URL blob si existe
    if (image.blobUrl) {
      URL.revokeObjectURL(image.blobUrl);
    }

    if (!image.buildingImageId) {
      // Imagen local no guardada, solo remover del array
      this.images.splice(index, 1);
      this.imagesChange.emit(this.images);
      return;
    }

    // Eliminar del servidor
    this.imageService.deleteImage(image.buildingImageId).subscribe({
      next: () => {
        this.images.splice(index, 1);
        this.imagesChange.emit(this.images);
      },
      error: (error) => {
        console.error('Error eliminando imagen:', error);
      }
    });
  }

  /**
   * Actualiza el texto alternativo de una imagen en el servidor
   */
  updateAltText(index: number) {
    const image = this.images[index];
    if (!image.buildingImageId) return;

    this.imageService.updateImageAltText(image.buildingImageId, image.altText).subscribe({
      next: () => {
        console.log('Texto alternativo actualizado');
      },
      error: (error) => {
        console.error('Error actualizando texto alternativo:', error);
      }
    });
  }

  /**
   * Sube todas las imágenes pendientes al servidor cuando se proporciona el buildingId
   */
  uploadPendingImages(buildingId: string): Promise<void> {
    const pendingImages = this.images.filter(img => img.file && !img.buildingImageId);

    if (pendingImages.length === 0) {
      return Promise.resolve();
    }

    const uploadPromises = pendingImages.map(image => {
      image.uploading = true;
      this.imagesChange.emit(this.images);

      return new Promise<void>((resolve, reject) => {
        this.imageService.uploadImage(buildingId, image.file!, image.fileName, image.altText).subscribe({
          next: (response) => {
            // Limpiar URL blob antes de actualizar con la URL del servidor
            if (image.blobUrl) {
              URL.revokeObjectURL(image.blobUrl);
              image.blobUrl = undefined;
            }

            // Actualizar imagen con datos del servidor
            image.buildingImageId = response.buildingImageId;
            image.url = this.imageService.getImageUrl(response.buildingImageId);
            image.uploading = false;
            image.file = undefined; // Limpiar archivo temporal
            image.size = response.size;

            // Si es la imagen de portada, establecerla en el servidor
            if (image.isCover) {
              this.imageService.setAsCoverImage(response.buildingImageId).subscribe({
                next: () => resolve(),
                error: (error) => {
                  console.error('Error estableciendo como portada:', error);
                  resolve(); // Continuar aunque falle establecer como portada
                }
              });
            } else {
              resolve();
            }
          },
          error: (error) => {
            image.error = 'Error al subir la imagen: ' + (error.error?.message || error.message);
            image.uploading = false;
            reject(error);
          }
        });
      });
    });

    return Promise.all(uploadPromises).then(() => {
      this.imagesChange.emit(this.images);
    });
  }

  /**
   * Maneja el evento de carga exitosa de imagen
   */
  onImageLoad(event: Event) {
    const target = event.target as HTMLImageElement;
    if (target.parentElement) {
      target.parentElement.classList.add('loaded');
    }
  }

  /**
   * Maneja el error al cargar una imagen
   */
  onImageError(event: Event, image: BuildingImage) {
    console.error('Error cargando imagen:', image.fileName, 'URL actual:', image.url);

    const img = event.target as HTMLImageElement;
    console.log('Estado de la imagen:', {
      naturalWidth: img.naturalWidth,
      naturalHeight: img.naturalHeight,
      complete: img.complete,
      currentSrc: img.currentSrc,
      buildingImageId: image.buildingImageId
    });

    // En producción, intentar directamente con la API externa
    const isProduction = this.isProduction();

    if (!image.imageError && image.buildingImageId && isProduction) {
      console.log('Intentando recargar imagen con URL directa de producción...');
      const directUrl = `https://devdemoapi1.azurewebsites.net/api/ImageStorage/download/${image.buildingImageId}`;

      if (img.src !== directUrl) {
        console.log('Cambiando URL de:', img.src, 'a:', directUrl);
        image.url = directUrl;
        this.imagesChange.emit(this.images);
        return;
      }
    }

    // Si aún falla, intentar con parámetros de cache busting
    if (!image.imageError && image.buildingImageId && !img.src.includes('_retry=')) {
      console.log('Intentando con cache busting...');
      const timestamp = Date.now();
      const cacheBustUrl = `${image.url}?_retry=${timestamp}&t=${timestamp}`;
      console.log('Nueva URL con cache busting:', cacheBustUrl);
      image.url = cacheBustUrl;
      this.imagesChange.emit(this.images);
      return;
    }

    // Último intento: usar headers específicos para Azure
    if (!image.imageError && image.buildingImageId && !img.src.includes('_final=')) {
      console.log('Último intento con headers específicos...');
      const finalUrl = `https://devdemoapi1.azurewebsites.net/api/ImageStorage/download/${image.buildingImageId}?_final=true&crossorigin=anonymous`;
      console.log('URL final:', finalUrl);
      image.url = finalUrl;
      this.imagesChange.emit(this.images);
      return;
    }

    // Si ya intentamos todas las opciones, mostrar placeholder
    console.log('Mostrando placeholder para imagen:', image.fileName);
    image.imageError = true;
    this.imagesChange.emit(this.images);
  }  /**
   * Convierte las imágenes pendientes a formato base64 para el endpoint unificado
   */
  async getImagesAsBase64(): Promise<any[]> {
    const pendingImages = this.images.filter(img => img.file && !img.buildingImageId);

    if (pendingImages.length === 0) {
      return [];
    }

    const imageFilesPromises = pendingImages.map(image => {
      return new Promise<any>((resolve, reject) => {
        if (!image.file) {
          reject(new Error('No hay archivo para convertir'));
          return;
        }

        const reader = new FileReader();
        reader.onload = (e) => {
          const base64String = (e.target?.result as string)?.split(',')[1]; // Quitar el prefijo data:image/...;base64,

          resolve({
            fileName: image.fileName,
            fileContent: base64String,
            contentType: image.file!.type,
            altText: image.altText || 'Imagen del edificio',
            isCoverImage: image.isCover || false
          });
        };
        reader.onerror = (error) => reject(error);
        reader.readAsDataURL(image.file);
      });
    });

    return Promise.all(imageFilesPromises);
  }

  /**
   * Detecta si estamos en producción
   */
  private isProduction(): boolean {
    if (typeof window === 'undefined') return false;
    const hostname = window.location.hostname;
    return hostname.includes('vercel.app') ||
           hostname.includes('vercel.com') ||
           hostname.includes('vercel.live') ||
           (hostname !== 'localhost' && hostname !== '127.0.0.1');
  }

  /**
   * Precarga las imágenes usando fetch para verificar si están disponibles
   */
  private preloadImagesForProduction(): void {
    console.log('Precargando imágenes para producción...');

    this.images.forEach((image, index) => {
      if (!image.buildingImageId) return;

      const directUrl = `https://devdemoapi1.azurewebsites.net/api/ImageStorage/download/${image.buildingImageId}`;

      fetch(directUrl, {
        method: 'HEAD',
        mode: 'cors',
        credentials: 'omit'
      })
      .then(response => {
        console.log(`Imagen ${image.fileName} - Status: ${response.status}, Headers:`, {
          contentType: response.headers.get('content-type'),
          contentLength: response.headers.get('content-length'),
          cacheControl: response.headers.get('cache-control'),
          accessControlAllowOrigin: response.headers.get('access-control-allow-origin')
        });

        if (!response.ok) {
          console.warn(`Imagen ${image.fileName} no disponible (${response.status})`);
          // Marcar como error si no está disponible
          setTimeout(() => {
            image.imageError = true;
            this.imagesChange.emit(this.images);
          }, 100);
        }
      })
      .catch(error => {
        console.error(`Error precargando imagen ${image.fileName}:`, error);
        // Intentar con el método original si falla el preload
      });
    });
  }

  /**
   * Método temporal para debugging de imágenes
   */
  debugImageLoading(): void {
    console.log('=== DEBUG IMÁGENES ===');
    console.log('buildingId:', this.buildingId);
    console.log('Total imágenes:', this.images.length);
    console.log('Es producción:', this.isProduction());
    console.log('Hostname:', window.location.hostname);

    this.images.forEach((img, index) => {
      console.log(`Imagen ${index + 1}:`, {
        fileName: img.fileName,
        buildingImageId: img.buildingImageId,
        url: img.url,
        isCover: img.isCover,
        imageError: img.imageError,
        size: img.size
      });

      // Intentar cargar la imagen manualmente para ver el error
      if (img.buildingImageId) {
        const testUrl = `https://devdemoapi1.azurewebsites.net/api/ImageStorage/download/${img.buildingImageId}`;
        console.log(`Probando URL directa para ${img.fileName}: ${testUrl}`);

        fetch(testUrl, { method: 'HEAD' })
          .then(response => {
            console.log(`${img.fileName} - Respuesta HTTP:`, response.status, response.statusText);
          })
          .catch(error => {
            console.error(`${img.fileName} - Error en fetch:`, error);
          });
      }
    });
    console.log('=====================');
  }

  ngOnDestroy() {
    // Limpiar todas las URLs blob al destruir el componente
    this.cleanupBlobUrls();
  }

  /**
   * Limpia todas las URLs blob para evitar memory leaks
   */
  private cleanupBlobUrls(): void {
    this.images.forEach(image => {
      if (image.blobUrl) {
        URL.revokeObjectURL(image.blobUrl);
      }
    });
  }
}
