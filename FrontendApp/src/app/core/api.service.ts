export interface RequestCreatorDto {
	buildingId: string;
	statusId: string;
	price: number;
	maintenancePrice: number;
}

export interface RequestGetterDto extends RequestCreatorDto {
	id: string;
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface DistrictGetterDto {
	id: string;
	name?: string;
	code?: string;
	zipcodes?: string[];
	country?: string;
	city?: string;
}

export interface ZipcodeGetterDto {
	id: string;
	code: string;
}

export interface StreetGetterDto {
  id: string;
  name: string;
  districts?: DistrictGetterDto[]; // Relación con distritos
}

export interface StatusGetterDto {
	id: string;
	name: string;
	description?: string;
}

export interface BuildingCompanyGetterDto {
	id: string;
	name: string;
	cif: string;
	website?: string;
}

export interface BuildingCreatorDto {
	name?: string;
	description?: string;
	// code se genera automáticamente en el backend
	doorway: string;
	floorCount?: number;
	yearBuilt?: number;
	price?: number | string;
	districtId?: string;
	streetId?: string;
	buildingCompanyId?: string;
	statusId?: string;
	energyCertificate?: string;
	hasElevator: boolean;

	// Campos adicionales para la creación automática del Address
	zipcodeId?: string;
	constructedAddress?: string;
	country?: string;
	city?: string;
}

export interface BuildingGetterDto extends BuildingCreatorDto {
	id: string;
}

export interface FloorGetterDto {
	id: string;
	floorNumber: number;
	buildingId: string;
}

export interface ApartmentCreatorDto {
	code: string;
	door: string;
	floorId: string;
}

export interface ApartmentGetterDto extends ApartmentCreatorDto {
	id: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
	private baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	// Request methods
	createRequest(dto: RequestCreatorDto): Observable<RequestGetterDto> {
		return this.http.post<RequestGetterDto>(`${this.baseUrl}/Request`, dto);
	}

	getDistricts(): Observable<DistrictGetterDto[]> {
		return this.http.get<DistrictGetterDto[]>(`${this.baseUrl}/District`);
	}

	getZipcodes(): Observable<ZipcodeGetterDto[]> {
		return this.http.get<ZipcodeGetterDto[]>(`${this.baseUrl}/Zipcode`);
	}

	getZipcodesByDistrict(districtId: string): Observable<ZipcodeGetterDto[]> {
		return this.http.get<ZipcodeGetterDto[]>(`${this.baseUrl}/Zipcode/by-district/${districtId}`);
	}

	getDistrictsByZipcode(zipcodeId: string): Observable<DistrictGetterDto[]> {
		return this.http.get<DistrictGetterDto[]>(`${this.baseUrl}/District/by-zipcode/${zipcodeId}`);
	}

	getStreets(): Observable<StreetGetterDto[]> {
		return this.http.get<StreetGetterDto[]>(`${this.baseUrl}/Street`);
	}

	getStreetsByDistrict(districtId: string): Observable<StreetGetterDto[]> {
		return this.http.get<StreetGetterDto[]>(`${this.baseUrl}/Street/by-district/${districtId}`);
	}

	// Status methods
	getStatuses(): Observable<StatusGetterDto[]> {
		return this.http.get<StatusGetterDto[]>(`${this.baseUrl}/Status`);
	}

	getStatusByName(name: string): Observable<StatusGetterDto | null> {
		return new Observable(observer => {
			this.getStatuses().subscribe(statuses => {
				const status = statuses.find(s => s.name.toLowerCase() === name.toLowerCase());
				observer.next(status || null);
				observer.complete();
			});
		});
	}

	// BuildingCompany methods
	getBuildingCompanies(): Observable<BuildingCompanyGetterDto[]> {
		return this.http.get<BuildingCompanyGetterDto[]>(`${this.baseUrl}/BuildingCompany`);
	}

	getBuildingCompanyById(id: string): Observable<BuildingCompanyGetterDto> {
		return this.http.get<BuildingCompanyGetterDto>(`${this.baseUrl}/BuildingCompany/${id}`);
	}

	// Building methods
	createBuilding(building: BuildingCreatorDto): Observable<BuildingGetterDto> {
		return this.http.post<BuildingGetterDto>(`${this.baseUrl}/Building`, building);
	}

	getBuildingById(id: string): Observable<BuildingGetterDto> {
		return this.http.get<BuildingGetterDto>(`${this.baseUrl}/Building/${id}`);
	}

	// Floor methods
	getFloorsByBuildingId(buildingId: string): Observable<FloorGetterDto[]> {
		return this.http.get<FloorGetterDto[]>(`${this.baseUrl}/Building/${buildingId}/floors`);
	}

	// Apartment methods
	createApartment(apartment: ApartmentCreatorDto): Observable<ApartmentGetterDto> {
		return this.http.post<ApartmentGetterDto>(`${this.baseUrl}/Apartment`, apartment);
	}

	getApartmentsByBuildingId(buildingId: string): Observable<ApartmentGetterDto[]> {
		return this.http.get<ApartmentGetterDto[]>(`${this.baseUrl}/Building/${buildingId}/apartments`);
	}
}
