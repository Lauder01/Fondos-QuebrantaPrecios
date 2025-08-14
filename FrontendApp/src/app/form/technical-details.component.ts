import { Component, Output, EventEmitter } from '@angular/core';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-technical-details',
  templateUrl: './technical-details.component.html',
  standalone: true,
  imports: [FormsModule]
})
export class TechnicalDetailsComponent {
  technicalDetails = {
    floorCount: 0,
    yearBuilt: 0,
    price: 0,
    energyCertificate: ''
  };

  @Output() dataChange = new EventEmitter<any>();

  onInputChange() {
    this.dataChange.emit(this.technicalDetails);
  }
}
