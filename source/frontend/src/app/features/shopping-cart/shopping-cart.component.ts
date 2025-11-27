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
  cartService = inject(CartService);
  cart$ = this.cartService.cartItems$;
  subtotal = 0;

  ngOnInit() {
    this.cartService.loadCart();
    this.cart$.subscribe(cart => {
      this.subtotal = cart?.total ?? 0;
    });
  }

  removeCart(itemId: number) {
    this.cartService.removeFromCart(itemId);
  }
}
