import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartItem, ShippingInfo } from '../../models/cart.models';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-receipt',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css',
})
export class ReceiptComponent implements OnInit {
  private shipService = inject(ShippingService);
  private cartService = inject(CartService);

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

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => {
      this.cart = items;
    });
  }
}
