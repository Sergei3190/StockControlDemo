import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IPersonPhotos } from '../../interfaces/person-photo/person-photos.interface';
import { IPersonPhotoItem } from '../../interfaces/person-photo/person-photo-item.interface';
import { IPersonPhotoFilter } from '../../interfaces/person-photo/person-photo-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class PersonPhotosService extends CrudService<IPersonPhotos, IPersonPhotoItem, IPersonPhotoFilter> {
  private readonly api: string = 'pc/api/v1/person-photos';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getPersonPhotos(filter: IPersonPhotoFilter): Observable<IPersonPhotos | IError> {
    return this.getList(this.api, filter);
  }

  getPersonPhotoById(id: Guid): Observable<IPersonPhotoItem | IError> {
    return this.getById(this.api, id);
  }

  getPersonPhotoByCardId(cardId: Guid): Observable<IPersonPhotoItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IPersonPhotoItem>(`${this.bffUrl}/${this.api}/card/${cardId}`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  createPersonPhoto(dto: IPersonPhotoItem): Observable<IPersonPhotoItem | IError> {
    return this.create(this.api, dto);
  }

  updatePersonPhoto(dto: IPersonPhotoItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deletePersonPhoto(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }
}
