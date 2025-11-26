import { Component } from '@angular/core';
import { CurrencyPipe, NgForOf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-receipt',
  imports: [CurrencyPipe, NgForOf, RouterLink],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css',
})
export class ReceiptComponent {
  shipping: any = {};

  cart: any[] = [];

  constructor(
    private shipService: ShippingService,
    private cartService: CartService
  ) {}

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => {
      this.cart = items;
    });
  }
}
