import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReceiptFilterModule } from './receipt-filter/receipt-filter.module';
import { ReceiptItemCreateModule } from './receipt-item-create/receipt-item-create.module';
import { ReceiptListModule } from './receipt-list/receipt-list.module';
import { ReceiptItemModule } from './receipt-item/receipt-item.module';

@NgModule({
  exports : [
    ReceiptListModule,
    ReceiptItemModule,
    ReceiptItemCreateModule,
    ReceiptFilterModule,
  ],
  imports: [
    CommonModule,
    ReceiptListModule.forRoot(),
    ReceiptItemModule,
    ReceiptItemCreateModule,
    ReceiptFilterModule,
  ]
})
export class ReceiptModule { }
