import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorHandlerService } from './error-handler.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    ErrorHandlerService
  ]
})

export class ErrorHandlerModule {}
