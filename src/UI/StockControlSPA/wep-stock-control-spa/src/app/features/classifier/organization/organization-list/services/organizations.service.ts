import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IOrganizations } from '../../../interfaces/organization/organizations.interface';
import { IOrganizationItem } from '../../../interfaces/organization/organization-item.interface';
import { IOrganizationFilter } from '../../../interfaces/organization/organization-filter.interface';
import { IBulkDeleteService } from 'src/app/shared/interfaces/services/bulk-delete-service.interface';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class OrganizationsService extends CrudService<IOrganizations, IOrganizationItem, IOrganizationFilter> 
    implements IBulkDeleteService{
  private readonly api: string = 'sk/api/v1/organizations';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getOrganizations(filter: IOrganizationFilter): Observable<IOrganizations | IError> {
    return this.getList(this.api, filter);
  }

  getOrganizationById(id: Guid): Observable<IOrganizationItem | IError> {
    return this.getById(this.api, id);
  }

  createOrganization(dto: IOrganizationItem): Observable<IOrganizationItem | IError> {
    return this.create(this.api, dto);
  }

  updateOrganization(dto: IOrganizationItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteOrganization(id: Guid): Observable<boolean | IError> {
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
