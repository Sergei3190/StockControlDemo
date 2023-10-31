import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, tap } from 'rxjs';
import { AppConfigurationService } from '../app-configuration/services/app-configuration.service';
import { IFilter } from '../../interfaces/filter.interface';
import { IPaginatedItems } from '../../interfaces/paginated-items.interface';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { HeadersService } from '../headers/headers.service';
import { StorageService } from '../storage/services/storage.service';
import { IError } from '../../interfaces/error.interface';

export abstract class SelectService<IPage extends IPaginatedItems, IQueryString extends IFilter> {
  protected bffUrl?: string;

  constructor(protected http: HttpClient,
    protected errorService: ErrorHandlerService,
    protected headersService: HeadersService,
    private configurationService: AppConfigurationService,
    private storageService: StorageService) {

    if (this.configurationService?.isReady){
      this.setBffUrl();
    }
    else {
      this.configurationService?.settingsLoaded$?.subscribe(_ => {
        this.setBffUrl();
      });
    }

    // на случай, если сервер упал, то чтобы без перезапуска страницы отправить запрос на восстановленный сервер, мы попытаемся извлечь 
    // нужные данные их хранилища сессии
    if (!this.bffUrl){
      this.bffUrl = this.storageService.retrieve("bffUrl");
    }

  }

  protected select(api: string, filter: IQueryString): Observable<IPage | IError> {
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

  private setBffUrl() {
    this.bffUrl = this.configurationService.serverSettings?.bffUrl;
    if (!this.bffUrl){
      throw new Error("bffUrl cannot be empty");
    }
  }
}
