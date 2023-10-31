import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteItemComponent } from './note-item.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { SideDrawerBaseModule } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.module';

@NgModule({
  declarations: [
    NoteItemComponent
  ],
  exports: [
    NoteItemComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatTooltipModule,
    MatSlideToggleModule,
    MatIconModule,
    SideDrawerBaseModule,
    RouterModule,
  ]
})
export class NoteItemModule {}
