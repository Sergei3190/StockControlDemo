import { Component, OnInit } from '@angular/core';
import { SideBarItem } from './sidebar/models/sidebar-item.model';
import { Subject, takeUntil, tap, switchMap } from 'rxjs';
import { AppConfigurationService } from './shared/modules/app-configuration/services/app-configuration.service';
import { AuthenticationService } from './shared/modules/authentication/services/authentication.service';
import { SignalrService } from './shared/modules/signalr/services/signalr.service';
import { NotificationSettingsService } from './shared/modules/notification-settings/services/notification-settings.service';
import { INotificationSettingFilter } from './shared/modules/notification-settings/interfaces/notification-setting-filter.interface';
import { StorageService } from './shared/modules/storage/services/storage.service';
import { INotificationSettings } from './shared/modules/notification-settings/interfaces/notification-settings.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  private readonly destroy$ = new Subject<void>();

  title = 'wep-stock-control-spa';
  userName = "";

  sideBarItems : SideBarItem[] = [
    {
      label: "Остатки",
      href: "stock-availability",
      iconClass: "fs-6 bi bi-hdd-stack",
      disabled: false
    },
    {
      label: "Движение товара",
      href: "product-flow",
      iconClass: "fs-6 bi bi-journal-check",
      disabled: false
    },
    {
      label: "Справочники",
      href: "classifiers",
      iconClass: "fs-6 bi bi-wallet2",
      disabled: false
    },
    {
      label: "Личный кабинет",
      href: "personal-cabinet",
      iconClass: "fs-6 bi bi-person",
      disabled: false
    },
    {
      label: "Уведомления",
      href: "notifications",
      iconClass: "fs-6 bi bi-broadcast-pin",
      disabled: false
    },
    {
      label: "Заметки",
      href: "notes",
      iconClass: "fs-6 bi bi-list-task",
      disabled: false
    },
  ];

  constructor(private configurationService: AppConfigurationService,
    private signalrService: SignalrService,
    private authService: AuthenticationService,
    private storageService: StorageService,
    private notificationSettingsService: NotificationSettingsService) {
  }

  ngOnInit(): void {
    this.configurationService.load();

    if (window.location.hash) {
      this.authService.AuthorizedCallback();
    }

    this.initSubscriptions();

    if (this.authService.UserData){
      this.userName = this.authService.UserData.name;
    }
  }

  onLogin(){
    if (!this.configurationService.apiUrl || this.configurationService?.apiUrl === ''){
      this.configurationService.load();
    }
    this.authService.Authorize();
  }

  onLogout(){
    this.authService.Logoff();
  }

  private initSubscriptions() {
    this.configurationService.settingsLoaded$
    .pipe(
      switchMap(_ => this.notificationSettingsService.getNotificationSettings({
        page: 1,
        pageSize: 100
      } as INotificationSettingFilter)),
      tap((settings) => this.notificationSettingsService.saveNotificationSettingsInStorage((settings as INotificationSettings).items)),
      takeUntil(this.destroy$))
    .subscribe();

    this.authService.authenticationChallenge$
      .pipe(takeUntil(this.destroy$))
      .subscribe(_ => {
        this.userName = this.authService.UserData.name;
      });
  }
}