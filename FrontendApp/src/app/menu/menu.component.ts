import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bg-light w-100">
      <div class="container-fluid">
        <ul class="navbar-nav w-100 d-flex flex-row justify-content-between">
          <li class="nav-item flex-fill text-center">
            <a class="nav-link" routerLink="/" routerLinkActive="active">Inicio</a>
          </li>
          <li class="nav-item flex-fill text-center">
            <a class="nav-link" routerLink="/buildings" routerLinkActive="active">Edificios</a>
          </li>
          <li class="nav-item flex-fill text-center">
            <a class="nav-link" routerLink="/districts" routerLinkActive="active">Distritos</a>
          </li>
          <li class="nav-item flex-fill text-center">
            <a class="nav-link" routerLink="/companies" routerLinkActive="active">Empresas</a>
          </li>
          <li class="nav-item flex-fill text-center">
            <a class="nav-link" routerLink="/form" routerLinkActive="active">Agregar Edificio</a>
          </li>
        </ul>
      </div>
    </nav>
  `
})
export class MenuComponent {}
