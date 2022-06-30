import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class CsrfHeaderInterceptor implements HttpInterceptor {
  public intercept(
    req: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const csrfHeaderName = 'X-CSRF';
    const csrfHeaderValue = 1;

    if (!req.headers.has(csrfHeaderName)) {
      req = req.clone({
        headers: req.headers.set(csrfHeaderName, String(csrfHeaderValue)),
      });
    }

    return next.handle(req);
  }
}
