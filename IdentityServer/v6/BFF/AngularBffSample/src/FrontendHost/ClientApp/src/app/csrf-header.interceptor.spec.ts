import { TestBed } from '@angular/core/testing';

import { CsrfHeaderInterceptor } from './csrf-header.interceptor';

describe('CsrfHeaderInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      CsrfHeaderInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: CsrfHeaderInterceptor = TestBed.inject(CsrfHeaderInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
