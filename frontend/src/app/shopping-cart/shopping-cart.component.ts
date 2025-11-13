import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {CurrencyPipe, NgForOf} from '@angular/common';

@Component({
  selector: 'app-shopping-cart',
  imports: [
    RouterLink,
    CurrencyPipe,
    NgForOf
  ],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css'
})
export class ShoppingCartComponent {

  /* placeholder until database is made*/
  shoppingCart=[
    {name: 'name1', price: 0.00},
    {name: 'name1', price: 0.00},
    {name: 'name1', price: 0.00}
  ];




  removeCart(item: any) {
    console.log(`${item.name} added to cart`);
  }
}
