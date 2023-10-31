import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReceiptFilterComponent } from './receipt-filter.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { SelectNomenclaturesModule } from 'src/app/shared/modules/select-nomenclatures/select-nomenclatures.module';
import { SelectOrganizationsModule } from 'src/app/shared/modules/select-organizations/select-organizations.module';
import { SelectWarehousesModule } from 'src/app/shared/modules/select-warehouses/select-warehouses.module';
import { SelectPartiesModule } from 'src/app/shared/modules/select-parties/select-parties.module';

@NgModule({
  declarations: [
    ReceiptFilterComponent
  ],
  exports: [
    ReceiptFilterComponent
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
export class ReceiptFilterModule { }
