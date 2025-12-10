import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderSummaryDto, OrderDetailDto } from '../../../models/order.models';
import { SaleService } from '../../../services/sales/sale.service';
import { ReceiptModalComponent } from '../../../shared/receipt-modal/receipt-modal.component';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe, ReceiptModalComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css',
})
export class OrdersComponent implements OnInit {
  receipts: OrderSummaryDto[] = [];
  selectedReceipt: OrderDetailDto | null = null;
  private saleService = inject(SaleService);
  ngOnInit(): void {
    this.saleService.getAllOrders().subscribe({
      next: orders => (this.receipts = orders),
      error: err => console.error('Failed to load orders:', err),
    });
  }

  openReceipt(receipt: OrderSummaryDto) {
    this.saleService.getOrderById(receipt.saleId).subscribe({
      next: orderDetail => (this.selectedReceipt = orderDetail),
      error: err => console.error('Failed to load order details:', err),
    });
  }

  closeReceipt() {
    this.selectedReceipt = null;
  }
}
