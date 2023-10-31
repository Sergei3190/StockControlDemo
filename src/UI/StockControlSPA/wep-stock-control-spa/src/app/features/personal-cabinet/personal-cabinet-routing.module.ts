import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonalCabinetCardComponent } from './personal-cabinet-card/personal-cabinet-card.component';

const routes: Routes = [
{
    path: '',
    title: 'Личный кабинет',
    component: PersonalCabinetCardComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class PersonalCabinetRoutingModule { }
