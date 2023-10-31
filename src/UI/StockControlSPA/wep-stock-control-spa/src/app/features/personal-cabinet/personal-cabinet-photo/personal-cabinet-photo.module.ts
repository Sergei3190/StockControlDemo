import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonPhotosService } from './services/person-photos.service';
import { CrudModule } from 'src/app/shared/modules/crud/crud.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { FileStorageModule } from 'src/app/shared/modules/file-storage/file-storage.module';
import { PersonalCabinetPhotoComponent } from './personal-cabinet-photo.component';

@NgModule({
  declarations: [
    PersonalCabinetPhotoComponent
  ],
  exports: [
    PersonalCabinetPhotoComponent
  ],
  imports: [
    CommonModule,
    FileStorageModule,
    CrudModule,
    ErrorHandlerModule,
    HeadersModule
  ]
})
export class PersonalCabinetPhotoModule {
  static forRoot(): ModuleWithProviders<PersonalCabinetPhotoModule> {
    return {
      ngModule: PersonalCabinetPhotoModule,
      
      providers: [
        PersonPhotosService
      ]
    };
  } 
 }
