import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { CartItem } from '../../models/cart.models';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-shopping-cart',
  imports: [RouterLink, CurrencyPipe, AsyncPipe],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css',
})
export class ShoppingCartComponent implements OnInit {
  cart: CartItem[] = [];
  cartService = inject(CartService);
  cart$ = this.cartService.cartItems$;
  subtotal = 0;

  removeCart(item: CartItem) {
    this.cartService.removeFromCart(item._id);
  }

  ngOnInit() {
    this.cart$.subscribe(items => {
      this.cart = items;
    });
    this.cartService.subtotal$.subscribe(total => {
      this.subtotal = total;
    });
  }
}
