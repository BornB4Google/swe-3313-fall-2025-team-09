import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderSummaryDto, OrderDetailDto } from '../../models/order.models';

@Injectable({
  providedIn: 'root',
})
export class SaleService {
  private http = inject(HttpClient);
  getAllOrders(): Observable<OrderSummaryDto[]> {
    return this.http.get<OrderSummaryDto[]>('/api/orders');
  }

  getOrderById(id: number): Observable<OrderDetailDto> {
    return this.http.get<OrderDetailDto>(`/api/orders/${id}`);
  }
}
