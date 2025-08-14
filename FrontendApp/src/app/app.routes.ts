
import { LandingComponent } from './landing/landing.component';

export const routes: Routes = [
	{ path: '', component: LandingComponent },
];
import { Routes } from '@angular/router';
import { DistrictsPageComponent } from './features/districts/districts.page';
import { FormMainComponent } from './features/form/form-main.component';

export const routes: Routes = [
	{ path: '', redirectTo: 'districts', pathMatch: 'full' },
	{ path: 'districts', component: DistrictsPageComponent },
	{ path: 'form', component: FormMainComponent }
];
