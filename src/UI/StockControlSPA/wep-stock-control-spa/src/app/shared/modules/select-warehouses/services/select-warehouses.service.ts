import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SelectService } from 'src/app/shared/modules/select/select.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { ISelectWarehouses } from '../interfaces/select-warehouses.interface';
import { ISelectWarehouseFilter } from '../interfaces/select-warehouse-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class SelectWarehousesService extends SelectService<ISelectWarehouses, ISelectWarehouseFilter> {
  private readonly api: string = 'sk/api/v1/select-warehouses';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  selectWarehouses(filter: ISelectWarehouseFilter): Observable<ISelectWarehouses | IError> {
    return this.select(`${this.api}`, filter);
  }
}
