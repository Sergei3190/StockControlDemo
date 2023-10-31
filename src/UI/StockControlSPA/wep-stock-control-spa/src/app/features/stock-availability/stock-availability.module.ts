import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StockAvailabilityRoutingModule } from './stock-availability-routing.module';
import { StockAvailabilityListModule } from './stock-availability-list/stock-availability-list.module';
import { StockAvailabilityFilterModule } from './stock-availability-filter/stock-availability-filter.module';
import { StockAvailabilityItemModule } from './stock-availability-item/stock-availability-item.module';

@NgModule({
  exports:[
    StockAvailabilityRoutingModule,
    StockAvailabilityListModule,
    StockAvailabilityItemModule,
    StockAvailabilityFilterModule
  ],
  imports: [
    CommonModule,
    StockAvailabilityRoutingModule,
    StockAvailabilityListModule.forRoot(),
    StockAvailabilityItemModule,
    StockAvailabilityFilterModule
  ]
})
export class StockAvailabilityModule { }
