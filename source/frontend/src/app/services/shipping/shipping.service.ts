import { Injectable } from '@angular/core';
import { ShippingInfo } from '../../models/cart.models';

@Injectable({
  providedIn: 'root',
})
export class ShippingService {
  shippingInfo: ShippingInfo = {
    name: '',
    address1: '',
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
  selectedOption: string = 'Ground';
}
