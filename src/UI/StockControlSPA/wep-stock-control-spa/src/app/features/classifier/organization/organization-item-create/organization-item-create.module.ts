import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganizationItemCreateComponent } from './organization-item-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    OrganizationItemCreateComponent
  ],
  exports: [
    OrganizationItemCreateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
  ]
})
export class OrganizationItemCreateModule { }
