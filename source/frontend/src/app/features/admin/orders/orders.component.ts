import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe, NgForOf, NgIf } from '@angular/common';
import { InventoryItem } from '../../../models/inventory.models';
import { RouterLink } from '@angular/router';
import { Sale } from '../../../models/sale.model';
import { SaleService } from '../../../services/sales/sale.service';

@Component({
  selector: 'app-orders',
  imports: [NgForOf, CurrencyPipe, NgIf, RouterLink, DatePipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css',
})
export class OrdersComponent implements OnInit {
  receipts: Sale[] = [];
  selectedReceipt: Sale | null = null;
  constructor(private saleService: SaleService) {}
  ngOnInit(): void {
    this.saleService.getAllOrders().subscribe({
      next: sales => (this.receipts = sales),
      error: err => console.error('Failed to load sales:', err),
    });
  }

  openReceipt(receipt: Sale) {
    this.selectedReceipt = receipt;
  }

  closeReceipt() {
    this.selectedReceipt = null;
  }
}
