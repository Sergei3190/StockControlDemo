import { of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { IError } from '../../interfaces/error.interface';

@Injectable()
export class ErrorHandlerService {

  constructor(private toastr: ToastrService) {
  }

  public handleError(errorResponse: any) {
    console.error(errorResponse);
    let errorMsg = '';

    if (errorResponse?.error instanceof ErrorEvent) {
      errorMsg = `Error: ${errorResponse.error.message}`;
    } 
    else if (errorResponse.error && !(errorResponse?.error instanceof ErrorEvent)){
      errorMsg = errorResponse.error.split('\r\n')[0];
    }
    else {
      errorMsg = errorResponse.message;
    }

    console.error(errorMsg);  
    this.toastr.error(errorMsg, 'ОШИБКА!');

    return of({status: errorResponse.status} as IError);
  }
}
