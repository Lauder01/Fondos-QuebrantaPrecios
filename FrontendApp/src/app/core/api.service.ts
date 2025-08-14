import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DistrictGetterDto {
	id: string;
	name?: string;
	zipCode?: string;
	country?: string;
	city?: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
	private baseUrl = 'https://172.30.137.209:7124/api';

	constructor(private http: HttpClient) {}

	getDistricts(): Observable<DistrictGetterDto[]> {
		return this.http.get<DistrictGetterDto[]>(`${this.baseUrl}/District`);
	}
}
// ...existing code from original location will be moved here...
