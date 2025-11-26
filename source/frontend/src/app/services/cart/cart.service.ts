import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cart: any[] = [];
  private cartItems = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItems.asObservable();

  private idCounter = 1;

  cartCount$ = this.cartItems$.pipe(
    map(items => items.length)
  );

  addToCart(item: any) {
    const cartItem = {
      ...item,
      _id: this.idCounter++
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

  isInCart(id: number):boolean {
    return this.cart.some(i => i.itemId === id);
  }


}
