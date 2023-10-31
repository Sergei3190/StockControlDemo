import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideDrawerContainerModule } from './side-drawer-container/side-drawer-container.module';
import { SideDrawerBaseModule } from './side-drawer-base/side-drawer-base.module';

@NgModule({
  imports: [
    CommonModule,
    SideDrawerContainerModule,
    SideDrawerBaseModule,
  ],
  exports: [
    SideDrawerContainerModule,
    SideDrawerBaseModule,
  ]
})

export class SideDrawerModule {}
