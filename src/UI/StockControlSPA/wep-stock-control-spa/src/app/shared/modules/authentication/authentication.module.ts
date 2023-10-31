import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from './services/authentication.service';

@NgModule({
  imports: [
    CommonModule
  ],
})

export class AuthenticationModule { 
  static forRoot(): ModuleWithProviders<AuthenticationModule> {
    return {
        ngModule: AuthenticationModule,
        providers: [
          AuthenticationService
        ],
    };
  }
}
