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
}
