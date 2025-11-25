import { Component } from '@angular/core';
import { CurrencyPipe, NgForOf} from "@angular/common";
import { RouterLink } from "@angular/router";
import { ShippingService } from '../../services/shipping/shipping.service';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-confirm',
    imports: [
        CurrencyPipe,
        NgForOf,
        RouterLink
    ],
  templateUrl: './confirm.component.html',
  styleUrl: './confirm.component.css'
})


export class ConfirmComponent {

  shipping: any = {};

  cart: any[] =[];



  constructor(
    private shipService: ShippingService,
    private cartService: CartService) {}
  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
    this.cartService.cartItems$.subscribe(items => {this.cart = items;});
  }

}
