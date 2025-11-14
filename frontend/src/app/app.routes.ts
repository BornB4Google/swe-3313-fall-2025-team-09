import { Routes } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { InventoryComponent } from './inventory/inventory.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { AccountComponent } from './account/account.component';
import { ConfirmComponent } from './confirm/confirm.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'inventory', component: InventoryComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'shoppingCart', component: ShoppingCartComponent },
  { path: 'account', component: AccountComponent },
  { path: 'confirm', component: ConfirmComponent },
  { path: '**', redirectTo: '' },
];


/*
- admin panel
  - sales report
  - add item
- cart changed to lines no glow
- your account page (picture)
-



*/
