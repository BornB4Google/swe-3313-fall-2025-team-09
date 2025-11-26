import { Injectable } from '@angular/core';
import {CartItem} from '../../models/cart.models';

@Injectable({
  providedIn: 'root'
})
export class OrderSummaryService {
  cartSubtotal: number = 0;
  shippingCost: number = 0;
  total: number = 0;
  tax: number = 0;

  updateSummary(subtotal: number, shipping: number) {
    this.cartSubtotal = subtotal;
    this.shippingCost = shipping;
    this.tax = (subtotal + shipping) * 0.06;
    this.total = subtotal + shipping + this.tax;


  }


  constructor() { }
}

export class OrderSummary {
}
