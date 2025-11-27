export interface receiptInfo {
  saleId: number;
  saleDateTime: string;
  subtotal: number;
  tax: number;
  shippingCost: number;
  total: number;
  shippingSpeed: string;

  street1: string;
  street2: string | null;
  city: string;
  state: string;
  zip: string;

  cardLast4: string;
  userId: number;
  customerName: string;
  customerEmail: string;

  items: OrderedItem[];
}
export interface OrderedItem {
  itemId: number;
  name: string;
  price: number;
  primaryPhotoUrl: string;
}
