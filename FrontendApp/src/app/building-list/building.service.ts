
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Building {
  id: string;
  name: string;
  description?: string;
  doorway: string;
  floorCount?: number;
  yearBuilt?: number;
  price?: number;
  districtId?: string;
  streetId?: string;
  buildingCompanyId?: string;
  energyCertificate?: string;
  hasElevator?: boolean;
  constructedAddress?: string;
  city?: string;
  country?: string;
  zipcodeId?: string;
  statusId?: string;
  // Campos calculados/mapeados del frontend
  districtName?: string;
  statusName?: string;
}

export interface BuildingListResult {
  items: Building[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class BuildingService {
  private apiUrl = `${environment.apiUrl}/Building/paged`;
  private apiDetailUrl = `${environment.apiUrl}/Building`;

  constructor(private http: HttpClient) {}

  getBuildings(
    page: number = 1,
    pageSize: number = 10,
    name?: string,
    districtId?: string,
    companyId?: string
  ): Observable<BuildingListResult> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    if (name) params = params.set('name', name);
    if (districtId) params = params.set('districtId', districtId);
    if (companyId) params = params.set('companyId', companyId);
    return this.http.get<BuildingListResult>(this.apiUrl, { params });
  }

  getBuildingById(id: string): Observable<Building> {
    return this.http.get<Building>(`${this.apiDetailUrl}/${id}`);
  }
}
