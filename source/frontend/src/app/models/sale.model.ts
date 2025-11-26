import { SaleItem } from "./sale-item.model";
import { User } from "./user.models";

export interface Sale {
  saleId: number;
  userId: number;
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

  user: User;
  items: SaleItem[];
}
