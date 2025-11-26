import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Sale } from '../../../models/sale.model';
import { SaleService } from '../../../services/sales/sale.service';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css',
})
export class OrdersComponent implements OnInit {
  receipts: Sale[] = [];
  selectedReceipt: Sale | null = null;
  private saleService = inject(SaleService);
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
