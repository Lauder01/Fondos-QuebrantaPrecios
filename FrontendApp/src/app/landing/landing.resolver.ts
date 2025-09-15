import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BuildingService } from '../building-list/building.service';
import { ApiService, DistrictGetterDto, StatusGetterDto } from '../core/api.service';

@Injectable({ providedIn: 'root' })
export class LandingResolver implements Resolve<any> {
  constructor(
    private buildingService: BuildingService,
    private apiService: ApiService
  ) {}

  resolve(): Observable<any> {
    return forkJoin({
      districts: this.apiService.getDistricts(),
      statuses: this.apiService.getStatuses(),
      buildings: this.buildingService.getBuildings(1, 3)
    }).pipe(
      map(({ districts, statuses, buildings }) => {
        const districtMap: { [id: string]: string } = {};
        const statusMap: { [id: string]: string } = {};
        (districts || []).forEach((d: DistrictGetterDto) => { if (d?.id) districtMap[d.id] = d.name || '-'; });
        (statuses || []).forEach((s: StatusGetterDto) => { if (s?.id) statusMap[s.id] = s.name || '-'; });
        const enrichedBuildings = (buildings.items || []).map((b: any) => ({
          ...b,
          name: (b?.name && b.name.trim()) ? b.name : (b?.constructedAddress && b.constructedAddress.trim() ? b.constructedAddress : 'Edificio sin dirección'),
          districtName: districtMap[b?.districtId || ''] || '-',
          statusName: statusMap[b?.statusId || ''] || 'Sin estado'
        }));
        return { buildings: enrichedBuildings };
      }),
      catchError(err => {
        console.error('Error en LandingResolver:', err);
        return of({ buildings: [] });
      })
    );
  }
}
