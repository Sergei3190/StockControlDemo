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
import { IReceipts } from '../../../interfaces/receipt/receipts.interface';
import { IReceiptItem } from '../../../interfaces/receipt/receipt-item.interface';
import { IReceiptFilter } from '../../../interfaces/receipt/receip-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class ReceiptsService extends CrudService<IReceipts, IReceiptItem, IReceiptFilter>
    implements IBulkDeleteService {
  private readonly api: string = 'sk/api/v1/receipts';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getReceipts(filter: IReceiptFilter): Observable<IReceipts | IError> {
    return this.getList(this.api, filter);
  }

  getReceiptById(id: Guid): Observable<IReceiptItem | IError> {
    return this.getById(this.api, id);
  }

  createReceipt(dto: IReceiptItem): Observable<IReceiptItem | IError> {
    return this.create(this.api, dto);
  }

  updateReceipt(dto: IReceiptItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteReceipt(id: Guid): Observable<boolean | IError> {
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
