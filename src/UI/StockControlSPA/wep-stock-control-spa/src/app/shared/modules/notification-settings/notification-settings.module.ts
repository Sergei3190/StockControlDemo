import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrudModule } from '../crud/crud.module';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';
import { NotificationSettingsService } from './services/notification-settings.service';

@NgModule({
  imports: [
    CommonModule,
    CrudModule,
    ErrorHandlerModule,
    HeadersModule
  ],
  providers: [
    NotificationSettingsService
  ]
})
export class NotificationSettingsModule { }
