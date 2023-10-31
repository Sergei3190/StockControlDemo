import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovingItemCreateComponent } from './moving-item-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { SelectNomenclaturesModule } from 'src/app/shared/modules/select-nomenclatures/select-nomenclatures.module';
import { SelectOrganizationsModule } from 'src/app/shared/modules/select-organizations/select-organizations.module';
import { SelectWarehousesModule } from 'src/app/shared/modules/select-warehouses/select-warehouses.module';
import { SelectPartiesModule } from 'src/app/shared/modules/select-parties/select-parties.module';

@NgModule({
  declarations: [
    MovingItemCreateComponent
  ],
  exports: [
    MovingItemCreateComponent
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
export class MovingItemCreateModule { }
