export interface OrderItemDto {
  itemId: number;
  name: string;
  price: number;
  primaryPhotoUrl: string;
}

export interface OrderSummaryDto {
  saleId: number;
  saleDateTime: string;
  total: number;
  itemCount: number;
  userId: number;
  customerName: string;
  customerEmail: string;
}

export interface OrderDetailDto {
  saleId: number;
  saleDateTime: string;
  subtotal: number;
  tax: number;
  shippingCost: number;
  total: number;
  shippingSpeed: string;
  street1: string;
  street2?: string | null;
  city: string;
  state: string;
  zip: string;
  cardLast4: string;
  userId: number;
  customerName: string;
  customerEmail: string;
  items: OrderItemDto[];
}

export interface CheckoutRequest {
  firstName: string;
  lastName: string;
  street1: string;
  street2?: string | null;
  city: string;
  state: string;
  zip: string;
  shippingSpeed: string;
  cardLast4: string;
  expiration: string;
}
