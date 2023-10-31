import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationSettingListComponent } from './notification-setting-list/notification-setting-list.component';

const routes: Routes = [
{
    path: '',
    title: 'Настройка уведомлений',
    component: NotificationSettingListComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class NotificationRoutingModule { }
