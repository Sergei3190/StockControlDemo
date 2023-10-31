import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { INotificationSettings } from '../interfaces/notification-settings.interface';
import { INotificationSettingItem } from '../interfaces/notification-setting-item.interface';
import { INotificationSettingFilter } from '../interfaces/notification-setting-filter.interface';
import { notificationSettings } from '../notification-settings';

@Injectable()
export class NotificationSettingsService extends CrudService<INotificationSettings, INotificationSettingItem, INotificationSettingFilter> {
  private readonly api: string = 'nt/api/v1/notification-settings';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getNotificationSettings(filter: INotificationSettingFilter): Observable<INotificationSettings | IError> {
    return this.getList(this.api, filter);
  }

  getNotificationSettingById(id: Guid): Observable<INotificationSettingItem | IError> {
    return this.getById(this.api, id);
  }

  createNotificationSetting(dto: INotificationSettingItem): Observable<INotificationSettingItem | IError> {
    return this.create(this.api, dto);
  }

  updateNotificationSetting(dto: INotificationSettingItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteNotificationSetting(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }

  public saveNotificationSettingsInStorage(settings?: INotificationSettingItem[]): void {
    if (!settings) {
      return;
    }

    this.storageService.store(notificationSettings.receipts.storageKey, 
      settings.find(s => s.notificationType?.name === notificationSettings.receipts.filterKey));

    this.storageService.store(notificationSettings.movings.storageKey, 
      settings.find(s => s.notificationType?.name === notificationSettings.movings.filterKey));

    this.storageService.store(notificationSettings.writeOffs.storageKey, 
      settings.find(s => s.notificationType?.name === notificationSettings.writeOffs.filterKey));
  }
}
