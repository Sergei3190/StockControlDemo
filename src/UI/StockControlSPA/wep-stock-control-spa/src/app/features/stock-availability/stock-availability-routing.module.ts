import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockAvailabilityListComponent } from './stock-availability-list/stock-availability-list.component';
import { StockAvailabilityItemComponent } from './stock-availability-item/stock-availability-item.component';

const routes: Routes = [
{
    path: '',
    title: 'Остатки',
    component: StockAvailabilityListComponent,
    children: [
      {
        path: ':id',
        title: 'Информация по остатку',
        component: StockAvailabilityItemComponent,
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StockAvailabilityRoutingModule { }
