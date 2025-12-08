import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { CartService } from '../services/cart/cart.service';
import { map, take } from 'rxjs';

export const checkoutGuard: CanActivateFn = () => {
  const cartService = inject(CartService);
  const router = inject(Router);

  return cartService.cartItems$.pipe(
    take(1),
    map(cart => {
      if (!cart || !cart.items || cart.items.length === 0) {
        return router.parseUrl('/shoppingCart');
      }
      return true;
    })
  );
};
