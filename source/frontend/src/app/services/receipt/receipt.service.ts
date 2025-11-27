import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InventoryItem } from '../../models/inventory.models';
import { receiptInfo } from '../../models/receipt.model';

@Injectable({
  providedIn: 'root',
})
export class ReceiptService {
  private http = inject(HttpClient);
  private lastOrder: receiptInfo | null = null;
  private lastOrderId: number | null = null;

  constructor() {}

  checkout(request: any): Observable<receiptInfo> {
    return this.http.post<receiptInfo>('/api/orders/checkout', request);
  }

  setLastOrder(order: receiptInfo) {
    this.lastOrder = order;
    this.lastOrderId = order.saleId;
  }

  getLastOrder() {
    return this.lastOrder;
  }

  getOrderById(id: number): Observable<receiptInfo> {
    return this.http.get<receiptInfo>(`/api/orders/${id}`);
  }
}
