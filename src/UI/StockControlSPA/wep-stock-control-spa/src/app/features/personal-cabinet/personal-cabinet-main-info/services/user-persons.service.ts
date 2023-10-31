import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IUserPersons } from '../../interfaces/user-person/user-persons.interface';
import { IUserPersonItem } from '../../interfaces/user-person/user-person-item.interface';
import { IUserPersonFilter } from '../../interfaces/user-person/user-person-filter.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class UserPersonsService extends CrudService<IUserPersons, IUserPersonItem, IUserPersonFilter> {
  private readonly api: string = 'pc/api/v1/user-persons';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getUserPersons(filter: IUserPersonFilter): Observable<IUserPersons | IError> {
    return this.getList(this.api, filter);
  }

  getUserPersonById(id: Guid): Observable<IUserPersonItem | IError> {
    return this.getById(this.api, id);
  }

  getUserPersonByCardId(cardId: Guid): Observable<IUserPersonItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IUserPersonItem>(`${this.bffUrl}/${this.api}/card/${cardId}`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  getUserPersonCurrentUser(): Observable<IUserPersonItem | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.get<IUserPersonItem>(`${this.bffUrl}/${this.api}/person-current-user`, options)
      .pipe(
        tap(response => response),
        catchError(error => this.errorService.handleError(error))
      );
  }

  createUserPerson(dto: IUserPersonItem): Observable<IUserPersonItem | IError> {
    return this.create(this.api, dto);
  }

  updateUserPerson(dto: IUserPersonItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteUserPerson(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }
}
