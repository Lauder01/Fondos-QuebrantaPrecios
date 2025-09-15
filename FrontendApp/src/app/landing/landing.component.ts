import { Router } from '@angular/router';


import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BuildingService, Building } from '../building-list/building.service';
import { CommonModule } from '@angular/common';
import { BuildingCardComponent } from '../building-card/building-card.component';
import { ApiService, DistrictGetterDto, StatusGetterDto } from '../core/api.service';
import { forkJoin, of } from 'rxjs';

@Component({
  selector: 'app-landing',
  standalone: true,
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
  imports: [CommonModule, BuildingCardComponent]
})
export class LandingComponent implements OnInit {
  buildings: any[] = [];

  constructor(private route: ActivatedRoute, private router: Router) {}
  goToBuildings() {
    this.router.navigate(['/buildings']);
  }

  ngOnInit(): void {
    const data = this.route.snapshot.data['data'];
    this.buildings = data?.buildings || [];
  }
}
