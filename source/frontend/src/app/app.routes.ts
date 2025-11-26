import { Routes } from '@angular/router';
import { LandingComponent } from './features/landing/landing.component';
import { LoginComponent } from './features/login/login.component';
import { SignupComponent } from './features/signup/signup.component';
import { InventoryComponent } from './features/inventory/inventory.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { ShoppingCartComponent } from './features/shopping-cart/shopping-cart.component';
import { AccountComponent } from './features/account/account.component';
import { ConfirmComponent } from './features/confirm/confirm.component';
import { ReceiptComponent } from './features/receipt/receipt.component';
import { ADMIN_ROUTES } from './features/admin/admin.routes';
import { redirectIfAuthenticatedGuard } from './guards/redirect-if-authenticated.guard';
import { redirectIfUnauthenticatedGuard } from './guards/redirect-if-unauthenticated.guard';
import { adminGuard } from './guards/admin.guard';
export const routes: Routes = [
  { path: '', component: LandingComponent, canActivate: [redirectIfAuthenticatedGuard] },
  { path: 'login', component: LoginComponent, canActivate: [redirectIfAuthenticatedGuard] },
  { path: 'signup', component: SignupComponent, canActivate: [redirectIfAuthenticatedGuard] },
  {
    path: 'inventory',
    component: InventoryComponent,
    canActivate: [redirectIfUnauthenticatedGuard],
  },
  { path: 'checkout', component: CheckoutComponent, canActivate: [redirectIfUnauthenticatedGuard] },
  {
    path: 'shoppingCart',
    component: ShoppingCartComponent,
    canActivate: [redirectIfUnauthenticatedGuard],
  },
  { path: 'account', component: AccountComponent, canActivate: [redirectIfUnauthenticatedGuard] },
  { path: 'confirm', component: ConfirmComponent, canActivate: [redirectIfUnauthenticatedGuard] },
  { path: 'receipt', component: ReceiptComponent, canActivate: [redirectIfUnauthenticatedGuard] },
  { path: 'admin', children: ADMIN_ROUTES, canActivate: [adminGuard] },
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
