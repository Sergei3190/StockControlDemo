import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IStockAvailabilities } from '../../interfaces/stock-availabilities.interface';
import { IStockAvailabilityItem } from '../../interfaces/stock-availability-item.interface';
import { IStockAvailabilityFilter } from '../../interfaces/stock-availability-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class StockAvailabilitiesService {
  private bffUrl?: string;
  private readonly api: string = 'sk/api/v1/stock-availabilities';

  constructor(private http: HttpClient,
    private errorService: ErrorHandlerService,
    private headersService: HeadersService,
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

      if (!this.bffUrl){
        this.bffUrl = this.storageService.retrieve("bffUrl");
      }
    }

  public getList(filter: IStockAvailabilityFilter): Observable<IStockAvailabilities | IError> {
    const options = {} as any;
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IStockAvailabilities>(`${this.bffUrl}/${this.api}`, { 
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

  public getById(id: Guid): Observable<IStockAvailabilityItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IStockAvailabilityItem>(`${this.bffUrl}/${this.api}/${id}`, options)
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
