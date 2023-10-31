import { HttpClient } from '@angular/common/http';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { IPaginatedItems } from '../../interfaces/paginated-items.interface';
import { IEntity } from '../../interfaces/entity.interface';
import { IFilter } from '../../interfaces/filter.interface';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { HeadersService } from '../headers/headers.service';
import { AppConfigurationService } from '../app-configuration/services/app-configuration.service';
import { StorageService } from '../storage/services/storage.service';
import { IError } from '../../interfaces/error.interface';

export abstract class CrudService<IPage extends IPaginatedItems, IItem extends IEntity, IQueryString extends IFilter> {
  protected bffUrl?: string;

  constructor(protected http: HttpClient,
     protected errorService: ErrorHandlerService,
     protected headersService: HeadersService,
     private configurationService: AppConfigurationService,
     protected storageService: StorageService) {

      if (this.configurationService?.isReady){
        this.setBffUrl();
      }
      else {
        this.configurationService?.settingsLoaded$?.subscribe(_ => {
          this.setBffUrl();
        });
      }

      if (!this.bffUrl){
        this.bffUrl = this.storageService.retrieve("bffUrl");
      }
     }

  protected getList(api: string, filter: IQueryString): Observable<IPage | IError> {
    const options = {} as any;
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IPage>(`${this.bffUrl}/${api}`, { 
      headers: options.headers,
      params: {
        filter: JSON.stringify(filter ? filter : {})
      } 
    })
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  protected getById(api: string, id: Guid): Observable<IItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IItem>(`${this.bffUrl}/${api}/${id}`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  protected create(api: string, dto: IItem): Observable<IItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.post<IItem>(`${this.bffUrl}/${api}`, dto, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  protected update(api: string, dto: IItem): Observable<boolean | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.patch<boolean>(`${this.bffUrl}/${api}`, dto, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  protected delete(api: string, id: Guid): Observable<boolean | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.delete<boolean>(`${this.bffUrl}/${api}/${id}`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  private setBffUrl() {
    this.bffUrl = this.configurationService.serverSettings?.bffUrl;
    if (!this.bffUrl){
      throw new Error("bffUrl cannot be empty");
    }
  }
}
