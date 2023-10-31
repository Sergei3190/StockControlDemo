import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StockAvailabilityFilterComponent } from './stock-availability-filter.component';
import { SelectPartiesModule } from 'src/app/shared/modules/select-parties/select-parties.module';
import { SelectNomenclaturesModule } from 'src/app/shared/modules/select-nomenclatures/select-nomenclatures.module';
import { SelectOrganizationsModule } from 'src/app/shared/modules/select-organizations/select-organizations.module';
import { SelectWarehousesModule } from 'src/app/shared/modules/select-warehouses/select-warehouses.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    StockAvailabilityFilterComponent
  ],
  exports: [
    StockAvailabilityFilterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    SelectPartiesModule.forRoot(),
    SelectNomenclaturesModule.forRoot(),
    SelectOrganizationsModule.forRoot(),
    SelectWarehousesModule.forRoot(),
  ]
})
export class StockAvailabilityFilterModule { }
