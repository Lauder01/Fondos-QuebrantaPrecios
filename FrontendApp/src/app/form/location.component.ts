import { Component, Output, EventEmitter } from '@angular/core';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  standalone: true,
  imports: [FormsModule]
})
export class LocationComponent {
  location = {
    districtId: '',
    streetId: '',
    doorway: '',
    apartmentId: ''
  };

  @Output() dataChange = new EventEmitter<any>();

  onInputChange() {
    this.dataChange.emit(this.location);
  }
}
