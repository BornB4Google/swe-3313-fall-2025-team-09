import { Injectable } from '@angular/core';
import { ShippingInfo } from '../../models/cart.models';

@Injectable({
  providedIn: 'root',
})
export class ShippingService {
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
  subtotal = 0;
  shippingCost = 0;
  selectedOption = 'Ground';

  isInfoComplete() {
    for (const info in this.shippingInfo) {
      if (info == null) {
        return false;
      }
    }
    return true;
  }
}
