import { Injectable } from '@angular/core';
import { ShippingInfo } from '../../models/cart.models';

@Injectable({
  providedIn: 'root',
})
export class ShippingService {
  shippingInfo: ShippingInfo = {
    firstName: '',
    lastName: '',
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
  subtotal = 0;
  shippingCost = 0;
  selectedOption = 'Ground';

  clearShippingInfo() {
    this.shippingInfo = {
      firstName: '',
      lastName: '',
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
    this.subtotal = 0;
    this.shippingCost = 0;
    this.selectedOption = 'Ground';
  }
}
