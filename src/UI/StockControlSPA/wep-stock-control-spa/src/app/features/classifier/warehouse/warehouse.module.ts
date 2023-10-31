import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WarehouseListModule } from './warehouse-list/warehouse-list.module';
import { WarehouseItemModule } from './warehouse-item/warehouse-item.module';
import { WarehouseItemCreateModule } from './warehouse-item-create/warehouse-item-create.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    WarehouseListModule.forRoot(),
    WarehouseItemModule,
    WarehouseItemCreateModule,
  ]
})
export class WarehouseModule { }
