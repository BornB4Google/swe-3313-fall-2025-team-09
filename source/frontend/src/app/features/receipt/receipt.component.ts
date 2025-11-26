import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartItem, ShippingInfo } from '../../models/cart.models';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';
import { OrderSummaryService } from '../../services/order/order-summary.service';

@Component({
  selector: 'app-receipt',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css',
})
export class ReceiptComponent implements OnInit {
  private shipService = inject(ShippingService);
  private cartService = inject(CartService);
  private orderSummary = inject(OrderSummaryService);

  shipping: ShippingInfo = {
    name: '',
    address1: '',
    city: '',
    state: '',
    zip: '',
    email: '',
    phone: '',
    card: '',
    exp: '',
    cvv: '',
  };

  cart: CartItem[] = [];
  shippingCost = 0;
  total = 0;
  subtotal = 0;
  tax = 0;

  get maskedCard(): string {
    const card = this.shipService.shippingInfo.card || '';
    const last4 = card.replace(/\s/g, '').slice(-4);
    return `**** **** **** ${last4}`;
  }

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => {
      this.cart = items;
    });
    this.shippingCost = this.orderSummary.shippingCost;
    this.subtotal = this.orderSummary.cartSubtotal;
    this.tax = this.orderSummary.tax;
    this.total = this.orderSummary.total;
  }
}
