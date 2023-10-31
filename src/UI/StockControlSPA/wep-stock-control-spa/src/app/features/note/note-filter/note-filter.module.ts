import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteFilterComponent } from './note-filter.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerAdapterModule } from 'src/app/shared/modules/mat-datepicker-adapter/mat-datepicker-adapter.module';


@NgModule({
  declarations: [
    NoteFilterComponent
  ],
  exports: [
    NoteFilterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatDatepickerAdapterModule.forRoot()
  ],
})
export class NoteFilterModule { }
