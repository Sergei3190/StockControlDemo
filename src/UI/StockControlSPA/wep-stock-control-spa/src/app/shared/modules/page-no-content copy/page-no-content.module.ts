import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageNoContentComponent } from './page-no-content.component';

@NgModule({
  declarations: [
    PageNoContentComponent
  ],
  exports: [
    PageNoContentComponent
  ],
  imports: [
    CommonModule
  ]
})
export class PageNoContentModule {}
