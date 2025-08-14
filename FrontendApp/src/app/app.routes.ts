import { Routes } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { DistrictsPageComponent } from './features/districts/districts.page';
import { FormMainComponent } from './features/form/form-main.component';

export const routes: Routes = [
	{ path: '', component: LandingComponent },
	{ path: 'districts', component: DistrictsPageComponent },
	{ path: 'form', component: FormMainComponent }
];
