import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectModule } from '../select/select.module';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';
import { SelectPartiesService } from './services/select-parties.service';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { PaginationModule } from '../pagination/pagination.module';
import { FormsModule } from '@angular/forms';
import { SelectPartiesComponent } from './select-parties.component';

@NgModule({
  declarations: [
    SelectPartiesComponent
  ],
  exports: [
    SelectPartiesComponent
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

export class SelectPartiesModule {
  static forRoot(): ModuleWithProviders<SelectPartiesModule> {
    return {
        ngModule: SelectPartiesModule,

        providers: [
          SelectPartiesService
        ]
    };
  }
}
