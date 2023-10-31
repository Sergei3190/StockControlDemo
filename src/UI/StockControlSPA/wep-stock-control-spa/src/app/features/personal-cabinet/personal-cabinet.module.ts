import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonalCabinetRoutingModule } from './personal-cabinet-routing.module';
import { PersonalCabinetCardModule } from './personal-cabinet-card/personal-cabinet-card.module';

@NgModule({
  imports: [
    CommonModule,
    PersonalCabinetRoutingModule,
    PersonalCabinetCardModule
  ],
  exports: [
    PersonalCabinetRoutingModule,
    PersonalCabinetCardModule,
  ]
})
export class PersonalCabinetModule { }
