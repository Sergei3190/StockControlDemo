import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WarehouseItemCreateComponent } from './warehouse-item-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    WarehouseItemCreateComponent
  ],
  exports: [
    WarehouseItemCreateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
  ]
})
export class WarehouseItemCreateModule { }
