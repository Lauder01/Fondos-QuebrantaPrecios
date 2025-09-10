import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CurrencyPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-building-card',
  standalone: true,
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './building-card.component.html',
  styleUrls: ['./building-card.component.css']
})
export class BuildingCardComponent {
  @Input() building: any;

  constructor(private router: Router) {}

  goToDetail() {
    this.router.navigate(['/buildings', this.building.id]);
  }

  // Método para determinar si mostrar la dirección separadamente
  shouldShowAddress(): boolean {
    if (!this.building?.constructedAddress) return false;

    // Si el nombre es igual a la dirección construida, no mostrar la dirección por separado
    const name = this.building.name?.trim().toLowerCase() || '';
    const address = this.building.constructedAddress?.trim().toLowerCase() || '';

    // También verificar si el nombre contiene "edificio sin dirección"
    if (name.includes('edificio sin dirección')) return true;

    return name !== address && address !== '';
  }

  // Método para obtener la clase CSS del badge según el status
  getStatusBadgeClass(): string {
    const statusName = this.building?.statusName?.toLowerCase() || '';

    switch (statusName) {
      case 'aceptado':
        return 'badge bg-success';
      case 'pendiente':
        return 'badge bg-warning';
      case 'rechazado':
        return 'badge bg-danger';
      case 'comprado':
        return 'badge bg-info';
      case 'registrado':
        return 'badge bg-primary';
      default:
        return 'badge bg-secondary';
    }
  }

  // Método para manejar errores de carga de imagen
  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    if (imgElement) {
      imgElement.style.display = 'none';
    }
  }
}
