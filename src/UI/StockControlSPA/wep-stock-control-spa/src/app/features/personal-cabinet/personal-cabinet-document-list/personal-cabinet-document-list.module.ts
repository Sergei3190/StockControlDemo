import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonDocumentsService } from './services/person-documents.service';
import { FileStorageModule } from 'src/app/shared/modules/file-storage/file-storage.module';
import { CrudModule } from 'src/app/shared/modules/crud/crud.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { PersonalCabinetDocumentListComponent } from './personal-cabinet-document-list.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { FormsModule } from '@angular/forms';
import { ConfirmDeleteModule } from 'src/app/shared/modules/confirm-delete/confirm-delete.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';

@NgModule({
  declarations: [
    PersonalCabinetDocumentListComponent
  ],
  exports: [
    PersonalCabinetDocumentListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatTooltipModule,
    MatCardModule,
    PageNotFoundModule,  
    PageNoContentModule,
    PageUnauthorizedModule, 
    FileStorageModule,
    ConfirmDeleteModule,
    CrudModule,
    ErrorHandlerModule,
    HeadersModule
  ]
})

export class PersonalCabinetDocumentListModule {
  static forRoot(): ModuleWithProviders<PersonalCabinetDocumentListModule> {
    return {
      ngModule: PersonalCabinetDocumentListModule,
      
      providers: [
        PersonDocumentsService
      ]
    };
  } 
 }