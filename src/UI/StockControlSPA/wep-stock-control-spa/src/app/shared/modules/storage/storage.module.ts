import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StorageService } from './services/storage.service';

@NgModule({
  imports: [
    CommonModule
  ],
})

export class StorageModule {
  static forRoot(): ModuleWithProviders<StorageModule> {
    return {
        ngModule: StorageModule,
        providers: [
          StorageService
        ],
    };
  }
}
