import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonalCabinetCardComponent } from './personal-cabinet-card.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { RouterModule } from '@angular/router';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PersonalCabinetMainInfoModule } from '../personal-cabinet-main-info/personal-cabinet-main-info.module';
import { PersonalCabinetDocumentListModule } from '../personal-cabinet-document-list/personal-cabinet-document-list.module';
import { PersonalCabinetPhotoModule } from '../personal-cabinet-photo/personal-cabinet-photo.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';

@NgModule({
  declarations: [
    PersonalCabinetCardComponent
  ],
  exports: [
    PersonalCabinetCardComponent,
    PersonalCabinetMainInfoModule,
    PersonalCabinetDocumentListModule,
    PersonalCabinetPhotoModule
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
    PersonalCabinetMainInfoModule.forRoot(),
    PersonalCabinetDocumentListModule.forRoot(),
    PersonalCabinetPhotoModule.forRoot()
  ]
})
export class PersonalCabinetCardModule {}
