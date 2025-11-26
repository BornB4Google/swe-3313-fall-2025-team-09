import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { CartService } from '../../services/cart/cart.service';
import { CartItem } from '../../models/cart.models';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule, CurrencyPipe],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css',
})
export class CheckoutComponent implements OnInit {
  private shipService = inject(ShippingService);
  private router = inject(Router);
  private cartService = inject(CartService);

  checkoutData = {
    name: '',
    address1: '',
    address2: '',
    city: '',
    state: '',
    zip: '',
    email: undefined as string | undefined,
    phone: undefined as string | undefined,
    card: undefined as string | undefined,
    exp: undefined as string | undefined,
    cvv: undefined as string | undefined,
  };

  subtotal = 0;
  shippingCost = 0;

  shippingOptions: Record<string, number> = {
    Overnight: 29.99,
    ThreeDay: 19.99,
    Ground: 0.0,
  };
  total = 0;

  updateShipping(option: string) {
    this.shippingCost = this.shippingOptions[option];
    this.calculateTotal();
  }

  calculateTotal() {
    this.total = this.subtotal + this.shippingCost;
  }

  confirmOrder(form: NgForm) {
    if (form.invalid) {
      Object.values(form.controls).forEach(control => {
        control.markAsTouched();
      });
      return;
    }
    this.shipService.shippingInfo = this.checkoutData;
    this.router.navigate(['/confirm']);
  }
  formatPhone(event: Event) {
    const target = event.target as HTMLInputElement;
    let value = target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    target.value = value;
    this.checkoutData.phone = value;
  }
  formatCC(event: Event) {
    const target = event.target as HTMLInputElement;
    let value = target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{4})(\d{4})(\d{4})(\d{4})/, '$1 $2 $3 $4');
    target.value = value;
    this.checkoutData.card = value;
  }
  formatNumOnly(event: Event) {
    const target = event.target as HTMLInputElement;
    const value = target.value.replace(/[^0-9]/g, '');
    target.value = value;
  }
  formatDate(event: Event) {
    const target = event.target as HTMLInputElement;
    let value = target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{2})(\d{4})/, '$1/$2');
    target.value = value;
  }

  cart: CartItem[] = [];

  ngOnInit() {
    this.cartService.cartItems$.subscribe(items => (this.cart = items));
    this.cartService.subtotal$.subscribe(value => {
      this.subtotal = value;
      this.calculateTotal();
    });
  }
}
