import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeadersService } from './headers.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    HeadersService
  ]
})

export class HeadersModule { 
}
