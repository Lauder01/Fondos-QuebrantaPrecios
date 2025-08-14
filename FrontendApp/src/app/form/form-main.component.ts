import { Component } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { GeneralInfoComponent } from './general-info.component';
import { LocationComponent } from './location.component';
import { TechnicalDetailsComponent } from './technical-details.component';

@Component({
  selector: 'app-form-main',
  templateUrl: './form-main.component.html',
  standalone: true,
  imports: [FormsModule, GeneralInfoComponent, LocationComponent, TechnicalDetailsComponent]
})
export class FormMainComponent {
  formData = {
    generalInfo: {},
    location: {},
    technicalDetails: {}
  };

  onGeneralInfoChange(data: any) {
    this.formData.generalInfo = data;
  }

  onLocationChange(data: any) {
    this.formData.location = data;
  }

  onTechnicalDetailsChange(data: any) {
    this.formData.technicalDetails = data;
  }

  submitForm() {
    console.log('Datos del formulario:', this.formData);
  }
}
