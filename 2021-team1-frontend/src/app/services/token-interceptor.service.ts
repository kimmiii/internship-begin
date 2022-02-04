import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { StateManagerService } from './state-manager.service';

@Injectable()
export class TokenInterceptorService implements HttpInterceptor {
  constructor(private stateManagerService: StateManagerService) {
  }

  // Token interceptor for security of backend
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.stateManagerService.token}`,
      },
    });
    return next.handle(request);
  }
}
