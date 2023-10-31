import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganizationListModule } from './organization-list/organization-list.module';
import { OrganizationItemModule } from './organization-item/organization-item.module';
import { OrganizationItemCreateModule } from './organization-item-create/organization-item-create.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    OrganizationListModule.forRoot(),
    OrganizationItemModule,
    OrganizationItemCreateModule,
  ]
})
export class OrganizationModule { }
