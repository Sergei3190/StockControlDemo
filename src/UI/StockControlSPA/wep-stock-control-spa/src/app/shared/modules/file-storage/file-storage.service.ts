import { Injectable } from '@angular/core';
import { AppConfigurationService } from '../app-configuration/services/app-configuration.service';
import { StorageService } from '../storage/services/storage.service';
import { HeadersService } from '../headers/headers.service';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { HttpClient } from '@angular/common/http';
import { Guid } from 'guid-ts';
import { Observable, Subject, catchError, takeUntil, tap } from 'rxjs';
import { IFileInfo } from '../../interfaces/file-info.interface';
import { IError } from '../../interfaces/error.interface';

@Injectable()
export class FileStorageService {
  private readonly destroy$ = new Subject<void>();
  private readonly api: string = 'fs/api/v1/file-storage';
  private bffUrl?: string;

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

      // на случай, если сервер упал, то чтобы без перезапуска страницы отправить запрос на восстановленный сервер, мы попытаемся извлечь 
      // нужные данные их хранилища сессии
      if (!this.bffUrl){
        this.bffUrl = this.storageService.retrieve("bffUrl");
      }
  }

  public upload(file: File): Observable<any> {
    const options = {} as any;
    this.headersService.setAuthorizationHeaders(options);

    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${this.bffUrl}/${this.api}/upload`, formData, {
        headers: options.headers,
        reportProgress: true,
      })
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  public getById(id: Guid): Observable<IFileInfo | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IFileInfo>(`${this.bffUrl}/${this.api}/${id}`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  public download(fileId: Guid, name?: string) {
    const options = {} as any;
    this.headersService.setAuthorizationHeaders(options);
    this.http.get(`${this.bffUrl}/${this.api}/download/${fileId}`, {
        headers: options.headers,
        responseType: 'blob' as 'json',
        observe: 'response'
      })
      .pipe(
        catchError(error => this.errorService.handleError(error)),
        takeUntil(this.destroy$))
      .subscribe((response: any) => {
        const headerFileName = decodeURI(response.headers.get('x-file-name'));
        const fileName = headerFileName.replace(/['"+]/g,'');
        const dataType = response.body.type;
        const binaryData = [response.body];
        const a = document.createElement('a');

        a.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));

        if (name) {
            a.download = name;
        } else {
          a.download = fileName;
        }

        document.body.appendChild(a);
        a.click();
    });
  }

  private setBffUrl() {
    this.bffUrl = this.configurationService.serverSettings?.bffUrl;
    if (!this.bffUrl){
      throw new Error("bffUrl cannot be empty");
    }
  }
}
