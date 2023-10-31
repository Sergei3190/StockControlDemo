import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SideDrawerContainerComponent } from './side-drawer-container.component';

@NgModule({
  declarations: [
    SideDrawerContainerComponent,
  ],
  imports: [
    CommonModule,
    MatSidenavModule,
  ],
  exports: [
    SideDrawerContainerComponent
  ]
})

export class SideDrawerContainerModule {}
