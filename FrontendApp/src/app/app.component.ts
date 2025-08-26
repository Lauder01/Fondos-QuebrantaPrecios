import { HeaderComponent } from './header/header.component';
import { MenuComponent } from './menu/menu.component';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HeaderComponent, MenuComponent, FooterComponent, RouterOutlet],
  template: `
    <app-header></app-header>
    <app-menu></app-menu>
    <div class="container mt-4">
      <router-outlet></router-outlet>
    </div>
    <app-footer></app-footer>
  `
})
export class AppComponent {}
