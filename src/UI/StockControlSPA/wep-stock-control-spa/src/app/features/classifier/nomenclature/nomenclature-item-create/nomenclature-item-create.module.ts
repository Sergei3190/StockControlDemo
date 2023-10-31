import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NomenclatureItemCreateComponent } from './nomenclature-item-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    NomenclatureItemCreateComponent
  ],
  exports: [
    NomenclatureItemCreateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
  ]
})
export class NomenclatureItemCreateModule { }
