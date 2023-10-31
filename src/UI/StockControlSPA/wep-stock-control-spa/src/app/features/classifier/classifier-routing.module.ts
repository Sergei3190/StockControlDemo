import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClassifierListComponent } from './classifier-list/classifier-list.component';
import { NomenclatureListComponent } from './nomenclature/nomenclature-list/nomenclature-list.component';
import { OrganizationListComponent } from './organization/organization-list/organization-list.component';
import { WarehouseListComponent } from './warehouse/warehouse-list/warehouse-list.component';
import { NomenclatureItemComponent } from './nomenclature/nomenclature-item/nomenclature-item.component';
import { OrganizationItemComponent } from './organization/organization-item/organization-item.component';
import { WarehouseItemComponent } from './warehouse/warehouse-item/warehouse-item.component';

// path должен совпадать с значением колонки mnemo в нижнем регистре таблицы бд справочники
const routes: Routes = [
{
    path: '',
    title: 'Справочники',
    component: ClassifierListComponent
  },
  {
    path: 'nomenclature',
    title: 'Номенклатура',
    component: NomenclatureListComponent,
    children: [
      {
        path: ':id',
        title: 'Редактирование',
        component: NomenclatureItemComponent,
      },
    ]
  },
  {
    path: 'organizations',
    title: 'Организации',
    component: OrganizationListComponent,
    children: [
      {
        path: ':id',
        title: 'Редактирование',
        component: OrganizationItemComponent,
      },
    ]
  },
  {
    path: 'warehouses',
    title: 'Склады',
    component: WarehouseListComponent,
    children: [
      {
        path: ':id',
        title: 'Редактирование',
        component: WarehouseItemComponent,
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ClassifierRoutingModule { }
