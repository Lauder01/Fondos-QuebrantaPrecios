import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
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
  size?: number;
  file?: File; // Archivo temporal antes de subir al servidor
}

@Component({
  selector: 'app-building-images',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './building-images.component.html',
  styleUrls: ['./building-images.component.css']
})
export class BuildingImagesComponent implements OnInit {
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
    this.imageService.getBuildingImages(this.buildingId).subscribe({
      next: (buildingImages) => {
        this.images = buildingImages.map(img => ({
          buildingImageId: img.buildingImageId,
          url: this.imageService.getImageUrl(img.buildingImageId),
          fileName: img.fileName,
          altText: img.altText,
          isCover: img.isCoverImage,
          size: img.size
        }));
        this.imagesChange.emit(this.images);
      },
      error: (error) => {
        console.error('Error cargando imágenes:', error);
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
}
