import { Component } from '@angular/core';
import { CurrencyPipe, NgForOf, NgIf } from '@angular/common';
import { InventoryItem } from '../../../models/inventory.models';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-orders',
  imports: [NgForOf, CurrencyPipe, NgIf, RouterLink],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css',
})
export class OrdersComponent {
  /*Dummy data temp*/
  receipts: any[] = [
    {
      id: 4001,
      date: '11/25/2025',
      items: [
        {
          name: 'Offshore Holdings Black Card',
          price: 2500000.0,
        },
        {
          name: 'Deep Slate Hoodie',
          price: 89.99,
        },
      ],
      shippingName: 'John Doe',
      shippingEmail: 'johndoe@gmail.com',
      shippingAddress1: '987 Deep Sea Drive',
      shippingCity: 'Atlantis',
      shippingState: 'FL',
      shippingZip: '33101',
      subtotal: 2500000.0 + 89.99 * 2,
      tax: 150000.0,
      total: 2500000.0 + 89.99 * 2 + 150000.0,
    },
  ];
  selectedReceipt: any | null = null;

  openReceipt(receipt: any) {
    this.selectedReceipt = receipt;
  }

  closeReceipt() {
    this.selectedReceipt = null;
  }
}
