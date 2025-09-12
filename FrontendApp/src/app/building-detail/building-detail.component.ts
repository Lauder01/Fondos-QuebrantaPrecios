import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BuildingService, Building } from '../building-list/building.service';
import { ApiService, DistrictGetterDto, StatusGetterDto, ApartmentGetterDto } from '../core/api.service';
import { Subject, forkJoin } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

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
  purchasing = false; // Para mostrar estado de compra
  purchaseSuccess = false; // Para mostrar mensaje de éxito temporal

  districts: DistrictGetterDto[] = [];
  statuses: StatusGetterDto[] = [];
  districtMap: { [id: string]: string } = {};
  statusMap: { [id: string]: string } = {};

  constructor(
    private route: ActivatedRoute,
    private buildingService: BuildingService,
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    // Los datos ya están resueltos por el resolver, solo necesitamos procesarlos
    this.route.data
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          const result = data['data'];
          if (!result) {
            this.building = null;
            this.loading = false;
            return;
          }

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

  purchaseBuilding(): void {
    if (!this.building?.id || this.purchasing) return;

    this.purchasing = true;

    // Obtener el ID del status "Pendiente"
    this.apiService.getStatusByName('Pendiente')
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (pendingStatus) => {
          if (!pendingStatus) {
            console.error('No se encontró el status "Pendiente"');
            this.purchasing = false;
            return;
          }

          if (!this.building) return;

          // Actualizar el edificio con el nuevo status
          this.buildingService.updateBuilding(this.building.id, {
            statusId: pendingStatus.id,
            // Incluir todas las propiedades requeridas del edificio
            name: this.building.name,
            description: this.building.description && this.building.description.trim() !== '' ? this.building.description : 'Edificio sin descripcion',
            doorway: this.building.doorway,
            floorCount: this.building.floorCount || 0,
            yearBuilt: this.building.yearBuilt || 1970,
            price: this.building.price || 0,
            districtId: this.building.districtId || '',
            streetId: this.building.streetId || '',
            buildingCompanyId: this.building.buildingCompanyId || '',
            energyCertificate: this.building.energyCertificate || '',
            hasElevator: this.building.hasElevator || false,
            constructedAddress: this.building.constructedAddress || '',
            city: this.building.city || '',
            country: this.building.country || '',
            zipcodeId: this.building.zipcodeId || '',
            apartmentCount: this.building.apartmentCount || 0
          })
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              // Llamar al endpoint de SpecuLab después de actualizar el edificio
              this.apiService.postToSpeculab(this.building!.id)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                  next: () => {
                    // Solo si el post a SpecuLab fue exitoso, cambiar el estado local y mostrar éxito
                    if (this.building) {
                      this.building.statusId = pendingStatus.id;
                      this.building.statusName = 'Pendiente';
                    }
                    this.purchasing = false;
                    this.purchaseSuccess = true;

                    // Forzar detección de cambios para actualizar la UI inmediatamente
                    this.cdr.detectChanges();

                    // Si el edificio pasa a 'Comprado', enviar apartamentos a CozyHouse
                    if (this.building?.statusName?.toLowerCase() === 'comprado') {
                      this.apiService.getApartmentsByBuildingId(this.building.id)
                        .pipe(takeUntil(this.destroy$))
                        .subscribe({
                          next: (apartments: ApartmentGetterDto[]) => {
                            if (apartments && apartments.length > 0) {
                              const requests = apartments.map(a => this.apiService.postToCozyhouse(a.id));
                              forkJoin(requests).subscribe({
                                next: (results) => {
                                  console.log('Todos los apartamentos enviados a CozyHouse:', results);
                                },
                                error: (err) => {
                                  console.error('Error enviando apartamentos a CozyHouse:', err);
                                }
                              });
                            }
                          },
                          error: (err) => {
                            console.error('Error obteniendo apartamentos para CozyHouse:', err);
                          }
                        });
                    }

                    // Ocultar mensaje de éxito después de 3 segundos
                    setTimeout(() => {
                      this.purchaseSuccess = false;
                      this.cdr.detectChanges();
                    }, 3000);

                    console.log('Edificio actualizado y enviado a SpecuLab');
                  },
                  error: (error) => {
                    // Si falla el post a SpecuLab, NO cambiar el estado ni mostrar éxito
                    console.error('Error al enviar a SpecuLab:', error);
                    this.purchasing = false;
                    this.cdr.detectChanges();
                  }
                });
            },
            error: (error) => {
              console.error('Error al actualizar el edificio:', error);
              this.purchasing = false;
              this.cdr.detectChanges();
            }
          });
        },
        error: (error) => {
          console.error('Error al obtener el status Pendiente:', error);
          this.purchasing = false;
          this.cdr.detectChanges();
        }
      });
  }
}
