import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppConfigurationService } from './services/app-configuration.service';

@NgModule({
  imports: [
    CommonModule
  ],
})

export class AppConfigurationModule {
  static forRoot(): ModuleWithProviders<AppConfigurationModule> {
    return {
        ngModule: AppConfigurationModule,
        providers: [
          AppConfigurationService
        ],
    };
  }
}
