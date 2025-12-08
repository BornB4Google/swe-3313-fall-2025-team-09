import { CanActivateFn } from '@angular/router';
import { ShippingService } from '../services/shipping/shipping.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const confirmGuard: CanActivateFn = (route, state) => {
  const customerInfo = inject(ShippingService)
  const router = inject(Router);

  const info = customerInfo.isInfoComplete();
  if(!info) {
    return router.parseUrl('/checkout');
  }
  return true;

};
