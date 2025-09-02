import { Routes } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { DistrictsPageComponent } from './features/districts/districts.page';
import { FormMainComponent } from './features/form/form-main.component';
import { BuildingListComponent } from './building-list/building-list.component';
import { BuildingDetailComponent } from './building-detail/building-detail.component';
import { BuildingDetailResolver } from './building-detail/building-detail.resolver';

import { ApartmentRegisterComponent } from './features/apartments/apartment-register.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'districts', component: DistrictsPageComponent },
  { path: 'form', component: FormMainComponent },
  {
    path: 'buildings/:id',
    component: BuildingDetailComponent,
    resolve: { data: BuildingDetailResolver }
  },
  { path: 'buildings', component: BuildingListComponent },
  { path: 'apartments/register/:buildingId', component: ApartmentRegisterComponent }
];
