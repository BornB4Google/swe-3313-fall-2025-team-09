import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs';
import { CartDto, CartItem } from '../../models/cart.models';
import { InventoryItem } from '../../models/inventory.models';
import { receiptInfo } from '../../models/receipt.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private http = inject(HttpClient);
  private cartItems = new BehaviorSubject<CartDto>({ items: [], total: 0 });
  cartItems$ = this.cartItems.asObservable();
  private subtotalSubject = new BehaviorSubject<number>(0);

  addToCart(id: number) {
    return this.http.post(`/api/cart/items`, { itemId: id }).subscribe(() => {
      this.loadCart();
    });
  }

  loadCart(): void {
    this.http.get<CartDto>('/api/cart').subscribe(cartDto => {
      this.cartItems.next(cartDto);
      this.subtotalSubject.next(cartDto.total);
    });
  }

  removeFromCart(id: number) {
    return this.http.delete(`/api/cart/items/${id}`).subscribe(() => {
      this.loadCart();
    });
  }

  clearCart() {
    this.cartItems.next({
      items: [],
      total: 0,
    });
    this.subtotalSubject.next(0);
  }
  getSubtotal() {
    return this.cartItems.value.total;
  }

  isInCart(itemId: number): boolean {
    return this.cartItems.value.items.some(i => i.itemId === itemId);
  }
}
