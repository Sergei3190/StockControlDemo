import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from 'guid-ts';
import { CrudService } from 'src/app/shared/modules/crud/crud.service';
import { ErrorHandlerService } from 'src/app/shared/modules/error-handler/error-handler.service';
import { HeadersService } from 'src/app/shared/modules/headers/headers.service';
import { AppConfigurationService } from 'src/app/shared/modules/app-configuration/services/app-configuration.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IPersonDocumentFilter } from '../../interfaces/person-document/person-document-filter.interface';
import { IPersonDocumentItem } from '../../interfaces/person-document/person-document-item.interface';
import { IPersonDocuments } from '../../interfaces/person-document/person-documents.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Injectable()
export class PersonDocumentsService extends CrudService<IPersonDocuments, IPersonDocumentItem, IPersonDocumentFilter> {
  private readonly api: string = 'pc/api/v1/person-documents';

  constructor(http: HttpClient,
     errorService: ErrorHandlerService,
     headersService: HeadersService,
     configurationService: AppConfigurationService,
     storageService: StorageService) {
      super(http, errorService, headersService, configurationService, storageService);
  }

  getPersonDocuments(filter: IPersonDocumentFilter): Observable<IPersonDocuments | IError> {
    return this.getList(this.api, filter);
  }

  getPersonDocumentById(id: Guid): Observable<IPersonDocumentItem | IError> {
    return this.getById(this.api, id);
  }

  createPersonDocument(dto: IPersonDocumentItem): Observable<IPersonDocumentItem | IError> {
    return this.create(this.api, dto);
  }

  updatePersonDocument(dto: IPersonDocumentItem): Observable<boolean | IError> {
    return this.update(this.api, dto);
  }

  deletePersonDocument(id: Guid): Observable<boolean | IError> {
    return this.delete(this.api, id);
  }
}
