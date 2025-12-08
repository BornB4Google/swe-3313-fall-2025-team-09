import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { receiptGuard } from './receipt.guard';

describe('receiptGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => receiptGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
