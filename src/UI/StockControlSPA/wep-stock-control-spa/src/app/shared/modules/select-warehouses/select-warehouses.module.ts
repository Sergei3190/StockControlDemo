import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectModule } from '../select/select.module';
import { ErrorHandlerModule } from '../error-handler/error-handler.module';
import { HeadersModule } from '../headers/headers.module';
import { SelectWarehousesService } from './services/select-warehouses.service';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { PaginationModule } from '../pagination/pagination.module';
import { SelectWarehousesComponent } from './select-warehouses.component';

@NgModule({
  declarations: [ 
    SelectWarehousesComponent,
  ],
  exports: [
    SelectWarehousesComponent
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

export class SelectWarehousesModule {
  static forRoot(): ModuleWithProviders<SelectWarehousesModule> {
    return {
        ngModule: SelectWarehousesModule,

        providers: [
          SelectWarehousesService
        ]
    };
  }
}
