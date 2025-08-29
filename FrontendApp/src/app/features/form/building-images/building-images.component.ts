import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface BuildingImage {
  url: string;
  fileName: string;
  altText: string;
  isCover: boolean;
  uploading?: boolean;
  error?: string;
}

@Component({
  selector: 'app-building-images',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './building-images.component.html',
  styleUrls: ['./building-images.component.css']
})
export class BuildingImagesComponent {
  @Input() images: BuildingImage[] = [];
  @Output() imagesChange = new EventEmitter<BuildingImage[]>();

  uploading = false;

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;
    const files = Array.from(input.files);
    files.forEach(file => this.uploadImage(file));
  }

  uploadImage(file: File) {
    // Aquí iría la lógica real de subida a Imgur (servicio aparte)
    const reader = new FileReader();
    const tempImage: BuildingImage = {
      url: '',
      fileName: file.name,
      altText: '',
      isCover: false,
      uploading: true
    };
    this.images.push(tempImage);
    this.imagesChange.emit(this.images);
    reader.onload = () => {
      // Simulación de subida exitosa
      setTimeout(() => {
        tempImage.url = reader.result as string;
        tempImage.uploading = false;
        this.imagesChange.emit(this.images);
      }, 1000);
    };
    reader.onerror = () => {
      tempImage.error = 'Error al leer la imagen';
      tempImage.uploading = false;
      this.imagesChange.emit(this.images);
    };
    reader.readAsDataURL(file);
  }

  setAsCover(index: number) {
    this.images.forEach((img, i) => img.isCover = i === index);
    this.imagesChange.emit(this.images);
  }

  removeImage(index: number) {
    this.images.splice(index, 1);
    this.imagesChange.emit(this.images);
  }
}
