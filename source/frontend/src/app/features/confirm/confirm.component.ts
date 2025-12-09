import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ShippingService } from '../../services/shipping/shipping.service';
import { CartService } from '../../services/cart/cart.service';
import { ShippingInfo } from '../../models/cart.models';
import { OrderSummaryService } from '../../services/order/order-summary.service';
import { ReceiptService } from '../../services/receipt/receipt.service';
import { CartDto } from '../../models/cart.models';
import { CheckoutRequest } from '../../models/order.models';

@Component({
  selector: 'app-confirm',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './confirm.component.html',
  styleUrl: './confirm.component.css',
})
export class ConfirmComponent implements OnInit {
  private shipService = inject(ShippingService);
  private cartService = inject(CartService);
  private receiptService = inject(ReceiptService);
  private orderSummary = inject(OrderSummaryService);
  private router = inject(Router);

  shippingInfo: ShippingInfo = {
    name: '',
    address1: '',
    address2: '',
    city: '',
    state: '',
    zip: '',
    email: '',
    phone: '',
    card: '',
    exp: '',
    cvv: '',
  };

  cart: CartDto | undefined;
  shippingCost = 0;
  total = 0;
  subtotal = 0;
  tax = 0;
  selectedOption = '';
  errorMessage: string | null = null;

  ngOnInit() {
    this.cartService.loadCart();
    this.cartService.cartItems$.subscribe(cartDto => (this.cart = cartDto));
    this.shippingInfo = this.shipService.shippingInfo;
    this.selectedOption = this.shipService.selectedOption;
    this.shippingCost = this.orderSummary.shippingCost;
    this.subtotal = this.orderSummary.cartSubtotal;
    this.tax = this.orderSummary.tax;
    this.total = this.orderSummary.total;
  }

  buildCheckout(): CheckoutRequest {
    const object: CheckoutRequest = {
      customerName: this.shippingInfo.name,
      street1: this.shippingInfo.address1,
      street2: this.shippingInfo.address2 || null,
      city: this.shippingInfo.city,
      state: this.shippingInfo.state,
      zip: this.shippingInfo.zip,
      shippingSpeed: this.shipService.selectedOption,
      cardLast4: this.maskedCard(this.shippingInfo.card),
      expiration: this.shippingInfo.exp,
    };
    return object;
  }

  maskedCard(card: string) {
    const last4 = card.replace(/\s/g, '').slice(-4);
    return last4;
  }
  saveReceipt() {
    const request = this.buildCheckout();
    this.receiptService.checkout(request).subscribe({
      next: result => {
        this.errorMessage = null;
        this.receiptService.setLastOrder(result);
        this.clearCart();
        this.shipService.clearShippingInfo();
        this.navigateToReceiptPage(result.saleId);
      },
      error: err => {
        console.error('Checkout failed', err);
        const serverError = typeof err?.error === 'string' ? err.error : err?.error?.message;
        const fallback = 'Failed to complete order. Please try again.';
        this.errorMessage = serverError?.trim()?.length ? serverError : fallback;
      },
    });
  }

  clearCart() {
    this.cartService.clearCart();
  }
  navigateToReceiptPage(id: number) {
    this.router.navigate(['/receipt', id]);
  }
}
