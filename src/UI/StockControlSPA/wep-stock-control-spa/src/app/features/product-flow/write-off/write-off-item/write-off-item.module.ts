import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WriteOffItemComponent } from './write-off-item.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SideDrawerBaseModule } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.module';
import { RouterModule } from '@angular/router';
import { SelectNomenclaturesModule } from 'src/app/shared/modules/select-nomenclatures/select-nomenclatures.module';
import { SelectOrganizationsModule } from 'src/app/shared/modules/select-organizations/select-organizations.module';
import { SelectWarehousesModule } from 'src/app/shared/modules/select-warehouses/select-warehouses.module';

@NgModule({
  declarations: [
    WriteOffItemComponent
  ],
  exports: [
    WriteOffItemComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTooltipModule,
    SideDrawerBaseModule,
    RouterModule,
    SelectNomenclaturesModule.forRoot(),
    SelectOrganizationsModule.forRoot(),
    SelectWarehousesModule.forRoot(),
  ]
})
export class WriteOffItemModule { }
