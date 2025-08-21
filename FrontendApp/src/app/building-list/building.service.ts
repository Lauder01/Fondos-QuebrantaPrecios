import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Building {
  id: string;
  name: string;
  description?: string;
  code: string;
  doorway: string;
  floorCount?: number;
  yearBuilt?: number;
  price?: number;
  districtId?: string;
  streetId?: string;
  buildingCompanyId?: string;
  // Agrega más campos según tu DTO
}

export interface BuildingListResult {
  items: Building[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class BuildingService {
  private apiUrl = '/api/building/paged';

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
}
