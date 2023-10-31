import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { AuthenticationService } from '../authentication/services/authentication.service';

@Injectable()
export class HeadersService {

  constructor(private authenticationService: AuthenticationService) {
  }

  public setAuthorizationHeaders(options : any): void {
    options["headers"] = new HttpHeaders()
      .append('authorization', 'Bearer ' + this.authenticationService.GetToken());
  }
}
