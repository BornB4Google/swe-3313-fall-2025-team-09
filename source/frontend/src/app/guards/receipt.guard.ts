import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { ReceiptService } from '../services/receipt/receipt.service';

export const receiptGuard: CanActivateFn = () => {
  const orderSummary = inject(ReceiptService);
  const router = inject(Router);

  const lastOrder = orderSummary.getLastOrder();
  if (lastOrder == null) {
    return router.parseUrl('/inventory');
  }
  return true;
};
