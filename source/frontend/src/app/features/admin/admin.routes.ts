import { Routes } from '@angular/router';
import { Component } from '@angular/core';
import { AdminComponent } from './admin.component';
import { UsersComponent } from './users/users.component';
import { ProductsComponent } from './products/products.component';
import { OrdersComponent } from './orders/orders.component';
import {DashboardComponent} from './dashboard/dashboard.component';

export const ADMIN_ROUTES: Routes = [
  {
    path:'',
    component: AdminComponent,
    children: [
      {path: 'dashboard', component: DashboardComponent},
      {path: 'users', component: UsersComponent},
      {path: 'products', component: ProductsComponent},
      {path: 'orders', component: OrdersComponent},
      {path: '',redirectTo: 'dashboard', pathMatch: 'full'}
    ]

  }

];
