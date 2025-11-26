import { Component, NgModule } from '@angular/core';
import { RouterLink } from "@angular/router";
import { FormsModule } from '@angular/forms';
import { ShippingService } from '../../services/shipping/shipping.service';
import { Router } from '@angular/router';
import {CommonModule, CurrencyPipe} from '@angular/common';
import {CartService} from '../../services/cart/cart.service';



@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule,
    CommonModule,
    CurrencyPipe
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})


export class CheckoutComponent {

  checkoutData = {
    name: '',
    address1: '',
    address2: '',
    city: '',
    state: '',
    zip: '',
    email: undefined,
    phone: undefined,
    card: undefined,
    exp: undefined,
    cvv: undefined
  };
  constructor(
    private shipService: ShippingService,
    private router: Router,
    private cartService: CartService
  ) {}

  subtotal: number = 0;
  shippingCost: number = 0;

  shippingOptions: any = {
    Overnight: 29.99,
    ThreeDay: 19.99,
    Ground: 0.00
  };
  total: number = 0;

  updateShipping(option: string) {
    this.shippingCost = this.shippingOptions[option];
    this.calculateTotal();
  }

  calculateTotal() {
    this.total = this.subtotal + this.shippingCost;
  }

  confirmOrder(form: any) {
    if (form.invalid) {
      Object.values(form.form.controls).forEach((control: any) => {
        control.markAsTouched();
      });
      return;
    }
    this.shipService.shippingInfo = this.checkoutData;
    this.router.navigate(['/confirm']);
  }
  formatPhone(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    event.target.value = value;
    this.checkoutData.phone = value;
  }
  formatCC(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{4})(\d{4})(\d{4})(\d{4})/, '$1 $2 $3 $4');
    event.target.value = value;
    this.checkoutData.card = value;
  }
  formatNumOnly(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');

    event.target.value = value;
  }
  formatDate(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{2})(\d{4})/, '$1/$2');
    event.target.value = value;
  }


  cart: any[] = [];


  ngOnInit() {
    this.cartService.cartItems$.subscribe(items => this.cart = items);
    this.cartService.subtotal$.subscribe(value => {
      this.subtotal = value;
      this.calculateTotal();
    });
  }



}

