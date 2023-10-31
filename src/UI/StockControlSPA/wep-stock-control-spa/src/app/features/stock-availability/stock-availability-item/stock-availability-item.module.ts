import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StockAvailabilityItemComponent } from './stock-availability-item.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SideDrawerBaseModule } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    StockAvailabilityItemComponent
  ],
  exports: [
    StockAvailabilityItemComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTooltipModule,
    SideDrawerBaseModule,
    RouterModule,
  ]
})
export class StockAvailabilityItemModule { }
