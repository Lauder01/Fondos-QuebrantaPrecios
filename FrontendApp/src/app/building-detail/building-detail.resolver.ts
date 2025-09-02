import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { timeout, catchError, take } from 'rxjs/operators';
import { BuildingService } from '../building-list/building.service';
import { ApiService } from '../core/api.service';

@Injectable({
  providedIn: 'root'
})
export class BuildingDetailResolver implements Resolve<any> {

  constructor(
    private buildingService: BuildingService,
    private apiService: ApiService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    const buildingId = route.paramMap.get('id');

    if (!buildingId) {
      return of(null);
    }

    return forkJoin({
      catalogs: forkJoin({
        districts: this.apiService.getDistricts().pipe(
          timeout(10000),
          catchError(() => of([])),
          take(1)
        ),
        statuses: this.apiService.getStatuses().pipe(
          timeout(10000),
          catchError(() => of([])),
          take(1)
        )
      }),
      building: this.buildingService.getBuildingById(buildingId).pipe(
        timeout(10000),
        catchError(() => of(null)),
        take(1)
      )
    }).pipe(
      catchError(() => of(null))
    );
  }
}
