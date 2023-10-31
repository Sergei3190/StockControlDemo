import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SideBarModule } from './sidebar/sidebar.module';
import { GlobalConfig, ToastrModule } from 'ngx-toastr';
import { AppConfigurationModule } from './shared/modules/app-configuration/app-configuration.module';
import { AuthenticationModule } from './shared/modules/authentication/authentication.module';
import { StorageModule } from './shared/modules/storage/storage.module';
import { SignalrModule } from './shared/modules/signalr/signalr.module';
import { NotificationSettingsModule } from './shared/modules/notification-settings/notification-settings.module';

const toastrConfig = {
    tapToDismiss: true,
    preventDuplicates: true,
    progressBar: true,
    enableHtml: true,
    timeOut: 10000,
    easeTime: 150,
} as GlobalConfig;

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        AppConfigurationModule.forRoot(),
        AuthenticationModule.forRoot(),
        StorageModule.forRoot(),    
        SignalrModule.forRoot(),
        NotificationSettingsModule, // нужен для начальной загрузки настроек уведомлений пользователя
        SideBarModule,
        ToastrModule.forRoot(toastrConfig),
        BrowserAnimationsModule
    ],
    providers: [{ provide: APP_BASE_HREF, useValue: "/app/wsc" }],
    bootstrap: [AppComponent],
})
export class AppModule { }
