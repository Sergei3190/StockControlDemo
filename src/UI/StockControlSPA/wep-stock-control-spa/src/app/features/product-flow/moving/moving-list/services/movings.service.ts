import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IBulkDeleteService } from 'src/app/shared/interfaces/services/bulk-delete-service.interface';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { IMovings } from '../../../interfaces/moving/movings.interface';
import { IMovingItem } from '../../../interfaces/moving/moving-item.interface';
import { IMovingFilter } from '../../../interfaces/moving/moving-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class MovingsService extends CrudService<IMovings, IMovingItem, IMovingFilter>
    implements IBulkDeleteService {
  private readonly api: string = 'sk/api/v1/movings';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getMovings(filter: IMovingFilter): Observable<IMovings | IError> {
    return this.getList(this.api, filter);
  }

  getMovingById(id: Guid): Observable<IMovingItem | IError> {
    return this.getById(this.api, id);
  }

  createMoving(dto: IMovingItem): Observable<IMovingItem | IError> {
    return this.create(this.api, dto);
  }

  updateMoving(dto: IMovingItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteMoving(id: Guid): Observable<boolean | IError> {
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
