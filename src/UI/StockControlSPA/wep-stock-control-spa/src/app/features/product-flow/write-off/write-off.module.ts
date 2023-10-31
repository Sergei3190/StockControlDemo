import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WriteOffFilterModule } from './write-off-filter/write-off-filter.module';
import { WriteOffListModule } from './write-off-list/write-off-list.module';
import { WriteOffItemModule } from './write-off-item/write-off-item.module';
import { WriteOffItemCreateModule } from './write-off-item-create/write-off-item-create.module';

@NgModule({
  exports : [
    WriteOffListModule,
    WriteOffItemModule,
    WriteOffItemCreateModule,
    WriteOffFilterModule,
  ],
  imports: [
    CommonModule,
    WriteOffListModule.forRoot(),
    WriteOffItemModule,
    WriteOffItemCreateModule,
    WriteOffFilterModule,
  ]
})
export class WriteOffModule { }
