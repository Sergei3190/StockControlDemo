import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NomenclatureListModule } from './nomenclature-list/nomenclature-list.module';
import { NomenclatureItemModule } from './nomenclature-item/nomenclature-item.module';
import { NomenclatureItemCreateModule } from './nomenclature-item-create/nomenclature-item-create.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NomenclatureListModule.forRoot(),
    NomenclatureItemModule,
    NomenclatureItemCreateModule,
  ]
})
export class NomenclatureModule { }
