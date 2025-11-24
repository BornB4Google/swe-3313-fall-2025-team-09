import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cart: any[] = [];
  private cartItems = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItems.asObservable();

  private idCounter = 1;

  addToCart(item: any) {
    const cartItem = {
      ...item,
      _id: this.idCounter++   // simple id
    };

    this.cart.push(cartItem);
    this.cartItems.next([...this.cart]);
    

  }

  removeFromCart(id: number) {
    this.cart = this.cart.filter(x => x._id !== id);
    this.cartItems.next([...this.cart]);
  }
/*
  clearCart() {
    this.cart = [];
    this.cartItems.next([]);
  }

 */
}
