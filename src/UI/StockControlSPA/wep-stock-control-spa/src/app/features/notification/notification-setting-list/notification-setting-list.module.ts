import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationSettingListComponent } from './notification-setting-list.component';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterModule } from '@angular/router';
import { PageNotFoundModule } from 'src/app/shared/modules/page-not-found/page-not-found.module';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { SearchInputModule } from 'src/app/shared/modules/search-input/search-input.module';
import { PageNoContentModule } from 'src/app/shared/modules/page-no-content copy/page-no-content.module';
import { PageUnauthorizedModule } from 'src/app/shared/modules/page-unauthorized/page-unauthorized.module';
import { NotificationSettingsModule } from 'src/app/shared/modules/notification-settings/notification-settings.module';

@NgModule({
  declarations: [
    NotificationSettingListComponent
  ],
  exports: [
    NotificationSettingListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatTooltipModule,
    MatCardModule,
    MatProgressBarModule,
    MatSlideToggleModule,
    RouterModule,
    PaginationModule,
    PageNotFoundModule,  
    PageNoContentModule,
    PageUnauthorizedModule, 
    SearchInputModule,
    NotificationSettingsModule
  ],
})

export class NotificationSettingListModule {}
