import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectModule } from '../select/select.module';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';
import { SelectNomenclaturesService } from './services/select-nomenclatures.service';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { PaginationModule } from '../pagination/pagination.module';
import { SelectNomenclaturesComponent } from './select-nomenclatures.component';

@NgModule({
  declarations: [
    SelectNomenclaturesComponent
  ],
  exports: [
    SelectNomenclaturesComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatAutocompleteModule,
    PaginationModule,
    SelectModule,
    ErrorHandlerModule,
    HeadersModule
  ],
})

export class SelectNomenclaturesModule {
  static forRoot(): ModuleWithProviders<SelectNomenclaturesModule> {
    return {
        ngModule: SelectNomenclaturesModule,

        providers: [
          SelectNomenclaturesService
        ]
    };
  }
}
