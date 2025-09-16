import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  styleUrls: ['./header.component.css'],
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  isMobileMenuOpen = false;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    // Solo ejecutar en el navegador (evitar errores de SSR)
    if (isPlatformBrowser(this.platformId)) {
      // Cerrar menú móvil con Escape globalmente
      window.addEventListener('keydown', (event: KeyboardEvent) => {
        if (this.isMobileMenuOpen && event.key === 'Escape') {
          this.closeMobileMenu();
        }
      });
      // Cerrar menú móvil al hacer clic fuera
      window.addEventListener('mousedown', (event: MouseEvent) => {
        const mobileNav = document.getElementById('mobileNav');
        if (this.isMobileMenuOpen && mobileNav && !mobileNav.contains(event.target as Node)) {
          this.closeMobileMenu();
        }
      });
    }
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
    // Animación simple: scroll bloqueado cuando menú abierto
    if (isPlatformBrowser(this.platformId)) {
      document.body.style.overflow = this.isMobileMenuOpen ? 'hidden' : '';
    }
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
    if (isPlatformBrowser(this.platformId)) {
      document.body.style.overflow = '';
    }
  }

  // Accesibilidad: cerrar menú con Escape desde el propio nav
  onMobileNavKeydown(event: KeyboardEvent) {
    if (event.key === 'Escape') {
      this.closeMobileMenu();
    }
  }

  // Interactividad: cerrar menú si se hace clic fuera del nav móvil
  onMobileNavMousedown(event: MouseEvent) {
    if (isPlatformBrowser(this.platformId)) {
      const mobileNav = document.getElementById('mobileNav');
      if (mobileNav && !mobileNav.contains(event.target as Node)) {
        this.closeMobileMenu();
      }
    }
  }
}
