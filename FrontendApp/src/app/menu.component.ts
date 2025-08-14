import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">FQP</a>
        <div class="collapse navbar-collapse">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0">
            <li class="nav-item">
              <a class="nav-link" routerLink="/districts" routerLinkActive="active">Distritos</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/form" routerLinkActive="active">Formulario</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  `
})
export class MenuComponent {}
