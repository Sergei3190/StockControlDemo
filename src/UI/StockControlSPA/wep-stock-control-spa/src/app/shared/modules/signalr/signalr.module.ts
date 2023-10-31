import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignalrService } from './services/signalr.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})

export class SignalrModule {
  static forRoot(): ModuleWithProviders<SignalrModule> {
    return {
        ngModule: SignalrModule,
        providers: [
          SignalrService
        ],
    };
  }
}
