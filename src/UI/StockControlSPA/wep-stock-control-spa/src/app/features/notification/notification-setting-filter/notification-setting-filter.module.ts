import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationSettingFilterComponent } from './notification-setting-filter.component';
import { NotificationTypesService } from './services/notification-types.service';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { SelectModule } from 'src/app/shared/modules/select/select.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';

@NgModule({
  declarations: [
    NotificationSettingFilterComponent
  ],
  exports: [
    NotificationSettingFilterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatNativeDateModule,
    MatSelectModule,
    MatAutocompleteModule,
    PaginationModule,
    SelectModule,
    ErrorHandlerModule,
    HeadersModule
  ],
})
export class NotificationSettingFilterModule {
  static forRoot(): ModuleWithProviders<NotificationSettingFilterModule> {
    return {
      ngModule: NotificationSettingFilterModule,
      
      providers: [
        NotificationTypesService
      ]
    };
  } 
}
