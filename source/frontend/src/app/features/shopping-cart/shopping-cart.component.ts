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

  cart: any[] = [];
  cartService = inject(CartService);
  cart$ = this.cartService.cartItems$;
  subtotal: number = 0;

  trackById(index: number, item: any) {
    return item._id;
  }

  removeCart(item: any) {
    this.cartService.removeFromCart(item._id);
  }

  ngOnInit() {
    this.cartService.cartItems$.subscribe(items => {
      this.cart = items;
      this.calculateSubtotal();
    })

  }

  calculateSubtotal(){
    this.subtotal = this.cart.reduce((num, item) => num + item.price, 0);
  }


}
