import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPersonsService } from './services/user-persons.service';
import { PersonalCabinetMainInfoComponent } from './personal-cabinet-main-info.component';
import { CrudModule } from 'src/app/shared/modules/crud/crud.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PersonalCabinetMainInfoComponent
  ],
  exports: [
    PersonalCabinetMainInfoComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CrudModule,
    ErrorHandlerModule,
    HeadersModule
  ]
})

export class PersonalCabinetMainInfoModule {
  static forRoot(): ModuleWithProviders<PersonalCabinetMainInfoModule> {
    return {
      ngModule: PersonalCabinetMainInfoModule,
      
      providers: [
        UserPersonsService
      ]
    };
  } 
}
