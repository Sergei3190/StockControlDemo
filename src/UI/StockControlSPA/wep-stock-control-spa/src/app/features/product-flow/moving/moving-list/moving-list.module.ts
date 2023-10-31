import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovingListComponent } from './moving-list.component';
import { MovingsService } from './services/movings.service';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { SideDrawerContainerModule } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
  declarations: [
    MovingListComponent
  ],
  exports: [
    MovingListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatMenuModule,
    MatDialogModule,
    MatProgressBarModule,
    RouterModule,
    PaginationModule,
    PageNotFoundModule,  
    PageNoContentModule,
    PageUnauthorizedModule, 
    SearchInputModule,
    SideDrawerContainerModule,
    ErrorHandlerModule,
    HeadersModule,
  ]
})
export class MovingListModule {
  static forRoot(): ModuleWithProviders<MovingListModule> {
    return {
      ngModule: MovingListModule,
      
      providers: [
        MovingsService
      ]
    };
  } 
 }
