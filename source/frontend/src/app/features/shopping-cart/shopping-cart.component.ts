import { Component, inject } from '@angular/core';
import { RouterLink } from "@angular/router";
import {AsyncPipe, CurrencyPipe, NgForOf} from '@angular/common';
import { CartService } from '../../services/cart/cart.service';


@Component({
  selector: 'app-shopping-cart',
  imports: [
    RouterLink,
    CurrencyPipe,
    NgForOf,
    AsyncPipe
  ],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css'
})
export class ShoppingCartComponent {

  /* placeholder until database is made*/
  cartService = inject(CartService);
  cart$ = this.cartService.cartItems$;

  trackById(index: number, item: any) {
    return item._id;
  }

  removeCart(item: any) {
    this.cartService.removeFromCart(item._id);
  }
}
