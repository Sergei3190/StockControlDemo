import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileStorageService } from './file-storage.service';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';

@NgModule({
  imports: [
    CommonModule,
    ErrorHandlerModule,
    HeadersModule
  ],
  providers: [
    FileStorageService
  ]
})
export class FileStorageModule { }
