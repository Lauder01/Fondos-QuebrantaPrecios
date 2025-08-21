import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';
import { ApiService, BuildingCompanyGetterDto } from '../../core/api.service';

@Component({
	selector: 'app-general-info',
	standalone: true,
	imports: [ReactiveFormsModule, CommonModule, NgClass],
	templateUrl: './general-info.component.html',
})
export class GeneralInfoComponent implements OnInit {
	@Input({ required: true }) formGroup!: FormGroup;

	allBuildingCompanies: BuildingCompanyGetterDto[] = [];
	filteredBuildingCompanies: BuildingCompanyGetterDto[] = [];
	showBuildingCompanyList = false;
	selectedBuildingCompany: BuildingCompanyGetterDto | null = null;

	// Control del modo del campo
	buildingCompanyMode: 'input' | 'dropdown' = 'input';

	// Bandera para evitar bucles infinitos
	private isUpdatingBuildingCompany = false;

	// Estados de validación
	buildingCompanyValid = true;
	buildingCompanyErrorMessage = '';

	constructor(private api: ApiService) {}

	ngOnInit() {
		this.api.getBuildingCompanies().subscribe(companies => {
			this.allBuildingCompanies = companies;
			this.filteredBuildingCompanies = companies;
		});

		this.formGroup.valueChanges.subscribe(() => {
			// Aquí puedes agregar lógica adicional si es necesaria
		});
	}

	onBuildingCompanyInput(value: string) {
		if (this.isUpdatingBuildingCompany) return;

		this.buildingCompanyMode = 'input';
		this.selectedBuildingCompany = null;
		this.buildingCompanyValid = true;
		this.buildingCompanyErrorMessage = '';

		// Sanitizar input
		const sanitizedValue = this.sanitizeInput(value, 255);
		if (sanitizedValue !== value) {
			this.isUpdatingBuildingCompany = true;
			this.formGroup.get('buildingCompanyId')?.setValue(sanitizedValue);
			this.isUpdatingBuildingCompany = false;
			return;
		}

		if (value.length === 0) {
			this.filteredBuildingCompanies = this.allBuildingCompanies;
			this.showBuildingCompanyList = false;
			return;
		}

		// Filtrar por nombre
		this.filteredBuildingCompanies = this.allBuildingCompanies.filter(company =>
			company.name.toLowerCase().includes(value.toLowerCase()) ||
			company.cif.toLowerCase().includes(value.toLowerCase())
		);

		this.showBuildingCompanyList = this.filteredBuildingCompanies.length > 0;

		// Si hay una coincidencia exacta, seleccionarla automáticamente
		const exactMatch = this.allBuildingCompanies.find(company =>
			company.name.toLowerCase() === value.toLowerCase()
		);

		if (exactMatch) {
			this.selectBuildingCompany(exactMatch);
		}
	}

	selectBuildingCompany(company: BuildingCompanyGetterDto) {
		this.selectedBuildingCompany = company;
		this.buildingCompanyMode = 'input';
		this.showBuildingCompanyList = false;
		this.buildingCompanyValid = true;
		this.buildingCompanyErrorMessage = '';

		this.isUpdatingBuildingCompany = true;
		this.formGroup.get('buildingCompanyId')?.setValue(company.id);
		this.isUpdatingBuildingCompany = false;
	}

	resetBuildingCompanySelection() {
		this.selectedBuildingCompany = null;
		this.buildingCompanyMode = 'input';
		this.showBuildingCompanyList = false;
		this.buildingCompanyValid = true;
		this.buildingCompanyErrorMessage = '';

		this.isUpdatingBuildingCompany = true;
		this.formGroup.get('buildingCompanyId')?.setValue('');
		this.isUpdatingBuildingCompany = false;
	}

	getInputValue(event: Event): string {
		return (event.target as HTMLInputElement).value;
	}

	private sanitizeInput(value: string, maxLength: number): string {
		return value.slice(0, maxLength);
	}

	onBuildingCompanyDropdownChange(event: Event) {
		const selectedId = this.getInputValue(event);
		if (selectedId) {
			const company = this.allBuildingCompanies.find(c => c.id === selectedId);
			if (company) {
				this.selectBuildingCompany(company);
			}
		}
	}
}
// ...existing code from original location will be moved here...
