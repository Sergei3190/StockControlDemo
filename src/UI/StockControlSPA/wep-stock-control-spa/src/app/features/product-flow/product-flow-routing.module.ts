import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductFlowTypeListComponent } from './product-flow-type-list/product-flow-type-list.component';
import { ReceiptListComponent } from './receipt/receipt-list/receipt-list.component';
import { MovingListComponent } from './moving/moving-list/moving-list.component';
import { WriteOffListComponent } from './write-off/write-off-list/write-off-list.component';
import { ReceiptItemComponent } from './receipt/receipt-item/receipt-item.component';
import { MovingItemComponent } from './moving/moving-item/moving-item.component';
import { WriteOffItemComponent } from './write-off/write-off-item/write-off-item.component';

export const productFlowUrls = {
  receipts: 'receipts',
  movings: 'movings',
  writeOffs: 'write-offs'
};

const routes: Routes = [
{
    path: '',
    title: 'Движение товара',
    component: ProductFlowTypeListComponent,
    children: [
      {
        path: productFlowUrls.receipts,
        title: 'Поступления',
        component: ReceiptListComponent,
        children: [
          {
            path: ':id',
            title: 'Редактирование',
            component: ReceiptItemComponent,
          },
        ]
      },
      {
        path: productFlowUrls.movings,
        title: 'Перемещения',
        component: MovingListComponent,
        children: [
          {
            path: ':id',
            title: 'Редактирование',
            component: MovingItemComponent,
          },
        ]
      },
      {
        path: productFlowUrls.writeOffs,
        title: 'Списания',
        component: WriteOffListComponent,
        children: [
          {
            path: ':id',
            title: 'Редактирование',
            component: WriteOffItemComponent,
          },
        ]
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ProductFlowRoutingModule { }
