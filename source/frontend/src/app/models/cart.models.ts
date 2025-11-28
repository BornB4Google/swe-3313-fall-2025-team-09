export interface CartItem {
  cartItemId: number;
  itemId: number;
  name: string;
  category: string;
  unitPrice: number;
  primaryPhotoUrl: string;
  //description: string;
}
export interface CartDto {
  items: CartItem[];
  total: number;
}

export interface ShippingInfo {
  name: string;
  address1: string;
  address2: string | null;
  city: string;
  state: string;
  zip: string;
  email: string;
  phone: string;
  card: string;
  exp: string;
  cvv: string;
}
