import { NgModule } from '@angular/core';
import { FeaturesRoutingModule } from './features-routing.module';
import { CommonModule } from '@angular/common';
import { FeaturesComponent } from './features.component';

@NgModule({
  declarations: [
    FeaturesComponent
  ],
  exports: [FeaturesComponent],
  imports: [
    CommonModule,
    FeaturesRoutingModule
  ]
})

export class FeaturesModule { }
