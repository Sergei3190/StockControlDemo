import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NoteListComponent } from './note-list.component';
import { NotesService } from './services/notes.service';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { SideDrawerContainerModule } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.module';
import { CrudModule } from 'src/app/shared/modules/crud/crud.module';
import { ErrorHandlerModule } from 'src/app/shared/modules/error-handler/error-handler.module';
import { HeadersModule } from 'src/app/shared/modules/headers/headers.module';
import { ConfirmDeleteModule } from 'src/app/shared/modules/confirm-delete/confirm-delete.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';

@NgModule({
  declarations: [
    NoteListComponent,
  ],
  exports: [
    NoteListComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatTooltipModule,
    MatCardModule,
    MatProgressBarModule,
    MatSidenavModule,
    RouterModule,
    DragDropModule,
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
  ],
})

export class NoteListModule {
  static forRoot(): ModuleWithProviders<NoteListModule> {
    return {
      ngModule: NoteListModule,
      
      providers: [
        NotesService, DatePipe
      ]
    };
  } 
}
