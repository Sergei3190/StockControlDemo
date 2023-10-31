import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassifierRoutingModule } from './classifier-routing.module';
import { ClassifierListModule } from './classifier-list/classifier-list.module';
import { NomenclatureModule } from './nomenclature/nomenclature.module';
import { OrganizationModule } from './organization/organization.module';
import { WarehouseModule } from './warehouse/warehouse.module';

@NgModule({
  exports: [
    ClassifierRoutingModule,
    ClassifierListModule,
    NomenclatureModule,
    OrganizationModule,
    WarehouseModule,
  ],
  imports: [
    CommonModule,
    ClassifierRoutingModule,
    ClassifierListModule.forRoot(),
    NomenclatureModule,
    OrganizationModule,
    WarehouseModule,
  ]
})

export class ClassifierModule { }
