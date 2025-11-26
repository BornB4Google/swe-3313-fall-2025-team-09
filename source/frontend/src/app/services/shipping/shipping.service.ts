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
  };
}
