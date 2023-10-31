import { CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';

export const authenticationGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);

  let authenticated = false;

  if (window.location.hash) {
    authService.AuthorizedCallback();
  }

  authenticated = authService.IsAuthorized as boolean;

  if (!authenticated) {
    authService.Authorize();
  }

  return authenticated;
};
