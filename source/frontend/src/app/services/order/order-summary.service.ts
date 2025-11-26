import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrderSummaryService {
  cartSubtotal = 0;
  shippingCost = 0;
  total = 0;
  tax = 0;

  updateSummary(subtotal: number, shipping: number) {
    this.cartSubtotal = subtotal;
    this.shippingCost = shipping;
    this.tax = (subtotal + shipping) * 0.06;
    this.total = subtotal + shipping + this.tax;
  }
}
