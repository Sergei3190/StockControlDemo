import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IWarehouses } from '../../../interfaces/warehouse/warehouses.interface';
import { IWarehouseItem } from '../../../interfaces/warehouse/warehouse-item.interface';
import { IWarehouseFilter } from '../../../interfaces/warehouse/warehouse-filter.interface';
import { IBulkDeleteService } from 'src/app/shared/interfaces/services/bulk-delete-service.interface';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class WarehousesService extends CrudService<IWarehouses, IWarehouseItem, IWarehouseFilter> 
    implements IBulkDeleteService{
  private readonly api: string = 'sk/api/v1/warehouses';

  // будут использоваться сингелтоны для всех зависимостей, кроме ErrorHandlerService, HeadersService, тк их мы внедряем в конкретный 
  // используемый модуль без указания forRoot, а так наш текущий модуль загружается лениво, то создаётся новый экземпляр
  // указанных сервисов
  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getWarehouses(filter: IWarehouseFilter): Observable<IWarehouses | IError> {
    return this.getList(this.api, filter);
  }

  getWarehouseById(id: Guid): Observable<IWarehouseItem | IError> {
    return this.getById(this.api, id);
  }

  createWarehouse(dto: IWarehouseItem): Observable<IWarehouseItem | IError> {
    return this.create(this.api, dto);
  }

  updateWarehouse(dto: IWarehouseItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteWarehouse(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }

  bulkDelete(ids: Guid[]): Observable<IBulkDeleteResult | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.post<IBulkDeleteResult>(`${this.bffUrl}/${this.api}/bulk-delete`, ids, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }
}
