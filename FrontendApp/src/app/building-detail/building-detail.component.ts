import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BuildingService, Building } from '../building-list/building.service';
import { ApiService, DistrictGetterDto, StatusGetterDto } from '../core/api.service';
import { forkJoin, of, Subject } from 'rxjs';
import { timeout, catchError, takeUntil, switchMap, take } from 'rxjs/operators';

@Component({
  selector: 'app-building-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CurrencyPipe],
  templateUrl: './building-detail.component.html',
  styleUrls: ['./building-detail.component.css']
})
export class BuildingDetailComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  buildingId: string | null = null;
  building: Building | null = null;
  loading = false;

  districts: DistrictGetterDto[] = [];
  statuses: StatusGetterDto[] = [];
  districtMap: { [id: string]: string } = {};
  statusMap: { [id: string]: string } = {};

  constructor(
    private route: ActivatedRoute,
    private buildingService: BuildingService,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        takeUntil(this.destroy$),
        switchMap(params => {
          this.buildingId = params.get('id');
          if (!this.buildingId) return of(null);

          this.loading = true;

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
            building: this.buildingService.getBuildingById(this.buildingId).pipe(
              timeout(10000),
              catchError(() => of(null)),
              take(1)
            )
          });
        })
      )
      .subscribe({
        next: (result) => {
          if (!result) return;

          // mapear catálogos
          const { districts, statuses } = result.catalogs;
          this.districts = districts || [];
          this.statuses = statuses || [];
          this.districtMap = {};
          this.statusMap = {};
          this.districts.forEach(d => { if (d?.id) this.districtMap[d.id] = d.name || '-'; });
          this.statuses.forEach(s => { if (s?.id) this.statusMap[s.id] = s.name || '-'; });

          // mapear edificio
          const building = result.building;
          if (building) {
            const pseudoName = building.name?.trim()
              ? building.name
              : (building.constructedAddress || 'Edificio sin nombre');

            this.building = {
              ...building,
              name: pseudoName,
              districtName: this.districtMap[building.districtId || ''] || 'Distrito no especificado',
              statusName: this.statusMap[building.statusId || ''] || 'Estado no especificado'
            };
          } else {
            this.building = null;
          }

          this.loading = false;
        },
        error: (err) => {
          console.error('Error en detalle:', err);
          this.building = null;
          this.loading = false;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getStatusBadgeClass(): string {
    const statusName = this.building?.statusName?.toLowerCase() || '';
    switch (statusName) {
      case 'aceptado': return 'badge bg-success fs-6';
      case 'pendiente': return 'badge bg-warning fs-6';
      case 'rechazado': return 'badge bg-danger fs-6';
      case 'comprado': return 'badge bg-info fs-6';
      case 'registrado': return 'badge bg-primary fs-6';
      default: return 'badge bg-secondary fs-6';
    }
  }

  getEnergyClass(): string {
    const cert = this.building?.energyCertificate?.toUpperCase() || '';
    switch (cert) {
      case 'A': return 'text-success fw-bold';
      case 'B': return 'text-info fw-bold';
      case 'C': return 'text-warning fw-bold';
      case 'D': return 'text-orange fw-bold';
      case 'E': return 'text-danger fw-bold';
      case 'F': return 'text-dark-red fw-bold';
      case 'G': return 'text-dark fw-bold';
      default: return 'text-muted';
    }
  }
}
