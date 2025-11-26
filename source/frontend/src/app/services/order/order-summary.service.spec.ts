import { TestBed } from '@angular/core/testing';

import { OrderSummaryService } from './order-summary.service';

describe('OrderSummaryService', () => {
  let service: OrderSummaryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrderSummaryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
