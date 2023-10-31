import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovingListModule } from './moving-list/moving-list.module';
import { MovingItemModule } from './moving-item/moving-item.module';
import { MovingItemCreateModule } from './moving-item-create/moving-item-create.module';
import { MovingFilterModule } from './moving-filter/moving-filter.module';

@NgModule({
  exports : [
    MovingListModule,
    MovingItemModule,
    MovingItemCreateModule,
    MovingFilterModule
  ],
  imports: [
    CommonModule,
    MovingListModule.forRoot(),
    MovingItemModule,
    MovingItemCreateModule,
    MovingFilterModule
  ]
})
export class MovingModule { }
