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


  private subtotalSubject = new BehaviorSubject<number>(0);
  subtotal$ = this.subtotalSubject.asObservable();

  addToCart(item: any) {
    const cartItem = {
      ...item,
      _id: this.idCounter++
    };

    this.cart.push(cartItem);
    this.cartItems.next([...this.cart]);
    this.updateSubtotal();


  }

  removeFromCart(id: number) {
    this.cart = this.cart.filter(x => x._id !== id);
    this.cartItems.next([...this.cart]);
    this.updateSubtotal();
  }
/*
  clearCart() {
    this.cart = [];
    this.cartItems.next([]);
  }
*/
  private updateSubtotal() {
    const subtotal = this.cart.reduce((sum, item) => sum + item.price, 0);
    this.subtotalSubject.next(subtotal);
  }

  isInCart(id: number):boolean {
    return this.cart.some(i => i.itemId === id);
  }


}
