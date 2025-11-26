import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';
import { CartItem, ShippingInfo } from '../../models/cart.models';

@Component({
  selector: 'app-confirm',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './confirm.component.html',
  styleUrl: './confirm.component.css',
})
export class ConfirmComponent implements OnInit {
  private shipService = inject(ShippingService);
  private cartService = inject(CartService);

  shipping: ShippingInfo = {
    name: '',
    address1: '',
    city: '',
    state: '',
    zip: '',
  };

  cart: CartItem[] = [];

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => {
      this.cart = items;
    });
  }
}
