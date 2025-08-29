import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
 // templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  template: `
    <header class="header">
      <div class="header-content">
        <h1 class="page-title">
          <a routerLink="/" routerLinkActive="active" class="brand-title">Fondos QuebrantaPrecios</a>
        </h1>
      </div>
    </header>
  `
})
export class HeaderComponent {}
