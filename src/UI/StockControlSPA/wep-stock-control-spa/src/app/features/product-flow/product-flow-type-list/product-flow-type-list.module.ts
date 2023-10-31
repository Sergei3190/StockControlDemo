import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductFlowTypeListComponent } from './product-flow-type-list.component';
import {MatTabsModule} from '@angular/material/tabs';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ReceiptModule } from '../receipt/receipt.module';
import { MovingModule } from '../moving/moving.module';
import { WriteOffModule } from '../write-off/write-off.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';

@NgModule({
  declarations: [
    ProductFlowTypeListComponent
  ],
  exports: [
    ProductFlowTypeListComponent,
    ReceiptModule,
    MovingModule,
    WriteOffModule,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatTabsModule,
    ReceiptModule,
    MovingModule,
    WriteOffModule,
    PaginationModule,
  ]
})
export class ProductFlowTypeListModule { }
