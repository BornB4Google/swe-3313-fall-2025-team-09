import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { InventoryItem } from '../../models/inventory.models';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private http = inject(HttpClient);

  getInventory(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>('/api/inventory');
  }

  addInventoryItem(item: InventoryItem): Observable<InventoryItem> {
    return this.http.post<InventoryItem>('/api/inventory', item);
  }

  searchInventory(query: string): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`/api/inventory/search?q=${encodeURIComponent(query)}`);
  }
}
