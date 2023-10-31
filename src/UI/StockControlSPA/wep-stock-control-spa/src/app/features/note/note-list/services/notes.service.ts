import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from 'guid-ts';
import { INotes } from '../../interfaces/notes.interface';
import { INoteItem } from '../../interfaces/note-item.interface';
import { INoteFilter } from '../../interfaces/note-filter.interface';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class NotesService extends CrudService<INotes, INoteItem, INoteFilter> {
  private readonly api: string = 'n/api/v1/notes';
  private readonly grpcApi: string = 'api/v1/notes-grpc';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getNotes(filter: INoteFilter): Observable<INotes | IError> {
    return this.getList(this.api, filter);
  }

  getNoteById(id: Guid): Observable<INoteItem | IError> {
    return this.getById(this.api, id);
  }

  createNote(dto: INoteItem): Observable<INoteItem | IError> {
    return this.create(this.api, dto);
  }

  updateNote(dto: INoteItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deleteNote(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }

  updateSort(dtoArray: INoteItem[]): Observable<boolean | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.patch<boolean>(`${this.bffUrl}/${this.api}/update-sort`, dtoArray, options);
  }

  updateSortGrpc(dtoArray: INoteItem[]): Observable<boolean | IError> {
    const options = {};
    this.headersService.setAuthorizationHeaders(options);
    return this.http.patch<boolean>(`${this.bffUrl}/${this.grpcApi}/update-sort`, dtoArray, options);
  }
}
