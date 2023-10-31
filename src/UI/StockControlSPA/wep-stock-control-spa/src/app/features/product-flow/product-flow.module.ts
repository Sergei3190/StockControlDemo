import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductFlowRoutingModule } from './product-flow-routing.module';
import { ProductFlowTypeListModule } from './product-flow-type-list/product-flow-type-list.module';

@NgModule({
  exports: [
    ProductFlowRoutingModule,
    ProductFlowTypeListModule,
  ],
  imports: [
    CommonModule,
    ProductFlowRoutingModule,
    ProductFlowTypeListModule,
  ]
})
export class ProductFlowModule { }
