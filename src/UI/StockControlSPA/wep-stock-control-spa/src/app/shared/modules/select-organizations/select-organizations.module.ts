import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectOrganizationsService } from './services/select-organizations.service';
import { SelectModule } from '../select/select.module';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { PaginationModule } from '../pagination/pagination.module';
import { SelectOrganizationsComponent } from './select-organizations.component';

@NgModule({
  declarations: [
    SelectOrganizationsComponent
  ],
  exports: [
    SelectOrganizationsComponent
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

export class SelectOrganizationsModule {
  static forRoot(): ModuleWithProviders<SelectOrganizationsModule> {
    return {
        ngModule: SelectOrganizationsModule,

        providers: [
          SelectOrganizationsService
        ]
    };
  }
}
