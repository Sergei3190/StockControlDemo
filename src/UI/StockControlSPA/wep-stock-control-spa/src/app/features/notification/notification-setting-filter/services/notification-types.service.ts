import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { INotificationTypes } from '../../interfaces/notification-type/notification-types.interface';
import { SelectService } from 'src/app/shared/modules/select/select.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { INotificationTypeFilter } from '../../interfaces/notification-type/notification-type-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class NotificationTypesService extends SelectService<INotificationTypes, INotificationTypeFilter> {
  private readonly api: string = 'nt/api/v1/notification-types';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  selectNotificationTypes(filter: INotificationTypeFilter): Observable<INotificationTypes | IError> {
    return this.select(`${this.api}/select`, filter);
  }
}
