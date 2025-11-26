import { InventoryItem } from './inventory.models';

export interface CartItem extends InventoryItem {
  _id: number;
}

export interface ShippingInfo {
  name: string;
  address1: string;
  city: string;
  state: string;
  zip: string;
  email: string;
  phone: string;
  card: string;
  exp: string;
  cvv: string;
}
