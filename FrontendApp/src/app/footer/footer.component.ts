import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <footer class="footer">
      <div class="footer-container">
        <div class="footer-content">
          <!-- Brand Section -->
          <div class="footer-brand">
            <h3 class="footer-brand-text">Fondos QuebrantaPrecios</h3>
            <div class="footer-divider"></div>
          </div>

          <!-- Main Content -->
          <div class="footer-main">
            <h4 class="footer-title">Acerca de nosotros</h4>
            <p class="footer-description">
              Somos un equipo especializado en conectar constructoras con nuevas oportunidades de inversión.
              A través de nuestra plataforma, centralizamos y organizamos edificios disponibles para la venta,
              ofreciendo un canal confiable y transparente para que las constructoras den visibilidad a sus proyectos.
              Nuestro objetivo es agilizar el proceso de registro, evaluación y compra, asegurando que cada oportunidad
              se convierta en una inversión de valor para la ciudad y para nuestra empresa.
            </p>
          </div>

          <!-- Bottom Bar -->
          <div class="footer-bottom">
            <p class="footer-copyright">
              © {{ currentYear }} Fondos QuebrantaPrecios. Todos los derechos reservados.
            </p>
          </div>
        </div>
      </div>
    </footer>
  `,
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
}
