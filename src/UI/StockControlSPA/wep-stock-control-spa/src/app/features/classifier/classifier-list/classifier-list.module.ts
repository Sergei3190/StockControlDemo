import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassifierListComponent } from './classifier-list.component';
import { ClassifiersService } from './services/classifiers.service';
import { SelectModule } from '../../../shared/modules/select/select.module';
import { ErrorHandlerModule } from '../../../shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';

@NgModule({
  declarations: [
    ClassifierListComponent
  ],
  exports: [
    ClassifierListComponent
  ],
  imports: [
    CommonModule,
    MatProgressBarModule,
    FormsModule,
    RouterModule,
    PaginationModule,
    PageNotFoundModule, 
    PageNoContentModule,
    PageUnauthorizedModule, 
    SearchInputModule,
    MatTooltipModule,
    MatCardModule,
    SelectModule,
    ErrorHandlerModule,
    HeadersModule
  ]
})

export class ClassifierListModule {
  static forRoot(): ModuleWithProviders<ClassifierListModule> {
    return {
      ngModule: ClassifierListModule,
      
      providers: [
        ClassifiersService
      ]
    };
  } 
}
