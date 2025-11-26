import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';
import { CartItem, ShippingInfo } from '../../models/cart.models';
import { OrderSummaryService } from '../../services/order/order-summary.service';



@Component({
  selector: 'app-confirm',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './confirm.component.html',
  styleUrl: './confirm.component.css',
})
export class ConfirmComponent implements OnInit {
  private shipService = inject(ShippingService);
  private cartService = inject(CartService);
  private orderSummary = inject(OrderSummaryService);

  shippingInfo: ShippingInfo = {
    name: '',
    address1: '',
    city: '',
    state: '',
    zip: '',
  };


  cart: CartItem[] = [];
  shippingCost: number = 0;
  total: number = 0;
  subtotal: number = 0;
  tax: number = 0;


  ngOnInit() {
    this.shippingInfo = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => (this.cart = items));
    this.shippingCost = this.orderSummary.shippingCost;
    this.subtotal = this.orderSummary.cartSubtotal;
    this.tax = this.orderSummary.tax;
    this.total = this.orderSummary.total;

  }
}
