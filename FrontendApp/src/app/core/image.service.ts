import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface BuildingImageDto {
  buildingImageId: string;
  fileName: string;
  altText: string;
  isCoverImage: boolean;
  hasImageData: boolean;
  size: number;
  downloadUrl: string;
}

export interface ImageUploadResponse {
  message: string;
  buildingImageId: string;
  size: number;
  downloadUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseUrl = '/api/ImageStorage';

  constructor(private http: HttpClient) { }

  /**
   * Sube una imagen al servidor
   */
  uploadImage(buildingId: string, file: File, fileName: string, altText: string): Observable<ImageUploadResponse> {
    const formData = new FormData();
    formData.append('buildingId', buildingId);
    formData.append('fileName', fileName);
    formData.append('altText', altText);
    formData.append('file', file);

    return this.http.post<ImageUploadResponse>(`${this.baseUrl}/upload`, formData);
  }

  /**
   * Obtiene todas las imágenes de un edificio
   */
  getBuildingImages(buildingId: string): Observable<BuildingImageDto[]> {
    return this.http.get<BuildingImageDto[]>(`${this.baseUrl}/building/${buildingId}`);
  }

  /**
   * Descarga una imagen por su ID (devuelve la URL para usar en img src)
   */
  getImageUrl(buildingImageId: string): string {
    // En producción, usar directamente la URL de la API externa para imágenes
    if (typeof window !== 'undefined' && window.location.hostname !== 'localhost') {
      return `https://devdemoapi1.azurewebsites.net/api/ImageStorage/download/${buildingImageId}`;
    }
    // En desarrollo, usar el proxy local
    return `${this.baseUrl}/download/${buildingImageId}`;
  }

  /**
   * Actualiza el texto alternativo de una imagen
   */
  updateImageAltText(buildingImageId: string, altText: string): Observable<any> {
    // Este endpoint lo necesitaremos crear si queremos actualizar solo el altText
    return this.http.patch(`${this.baseUrl}/update-alt/${buildingImageId}`, { altText });
  }

  /**
   * Elimina una imagen
   */
  deleteImage(buildingImageId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete/${buildingImageId}`);
  }

  /**
   * Establece una imagen como portada
   */
  setAsCoverImage(buildingImageId: string): Observable<any> {
    return this.http.patch(`${this.baseUrl}/set-cover/${buildingImageId}`, {});
  }
}
