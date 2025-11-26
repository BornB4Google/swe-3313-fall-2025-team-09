import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sale } from '../../models/sale.model';

@Injectable({
  providedIn: 'root',
})
export class SaleService {
  private http = inject(HttpClient);
  getAllOrders(): Observable<Sale[]> {
    return this.http.get<Sale[]>('/api/orders');
  }

  getOrderById(id: number): Observable<Sale> {
    return this.http.get<Sale>(`/api/orders/${id}`);
  }
}
