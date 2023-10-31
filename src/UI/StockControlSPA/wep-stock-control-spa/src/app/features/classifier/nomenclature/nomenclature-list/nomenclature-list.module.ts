import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NomenclatureListComponent } from './nomenclature-list.component';
import { NomenclaturesService } from './services/nomenclatures.service';
import { CrudModule } from 'src/app/shared/modules/crud/crud.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { SideDrawerContainerModule } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.module';
import { ConfirmDeleteModule } from 'src/app/shared/modules/confirm-delete/confirm-delete.module';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';

@NgModule({
  declarations: [
    NomenclatureListComponent
  ],
  exports: [
    NomenclatureListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatCheckboxModule,
    MatTooltipModule,
    MatMenuModule,
    MatProgressBarModule,
    RouterModule,
    PaginationModule,
    PageNotFoundModule,  
    PageNoContentModule,
    PageUnauthorizedModule, 
    SearchInputModule,
    SideDrawerContainerModule,
    ConfirmDeleteModule,
    CrudModule,
    ErrorHandlerModule,
    HeadersModule
  ]
})
export class NomenclatureListModule {
  static forRoot(): ModuleWithProviders<NomenclatureListModule> {
    return {
      ngModule: NomenclatureListModule,
      
      providers: [
        NomenclaturesService
      ]
    };
  } 
 }
