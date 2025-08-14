import { Component, Output, EventEmitter } from '@angular/core';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-general-info',
  templateUrl: './general-info.component.html',
  standalone: true,
  imports: [FormsModule]
})
export class GeneralInfoComponent {
  generalInfo = {
    name: '',
    description: '',
    code: '',
    doorway: '',
    buildingCompanyId: '',
    statusId: ''
  };

  @Output() dataChange = new EventEmitter<any>();

  onInputChange() {
    this.dataChange.emit(this.generalInfo);
  }
}
