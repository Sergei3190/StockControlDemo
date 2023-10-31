import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationRoutingModule } from './notification-routing.module';
import { NotificationSettingFilterModule } from './notification-setting-filter/notification-setting-filter.module';
import { NotificationSettingListModule } from './notification-setting-list/notification-setting-list.module';

@NgModule({
  imports: [
    CommonModule,
    NotificationRoutingModule,
    NotificationSettingListModule,
    NotificationSettingFilterModule.forRoot()
  ],
  exports: [
    NotificationRoutingModule,
    NotificationSettingListModule,
    NotificationSettingFilterModule
  ]
})
export class NotificationModule { }
