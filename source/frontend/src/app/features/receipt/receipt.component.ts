import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReceiptService } from '../../services/receipt/receipt.service';
import { receiptInfo } from '../../models/receipt.model';

@Component({
  selector: 'app-receipt',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css',
})
export class ReceiptComponent implements OnInit {
  private receiptService = inject(ReceiptService);
  private route = inject(ActivatedRoute);

  orderId: number | null = null;
  order: receiptInfo | null = null;

  ngOnInit() {
    const idFromUrl = this.route.snapshot.paramMap.get('id');
    this.orderId = idFromUrl ? Number(idFromUrl) : null;
    const cachedOrder = this.receiptService.getLastOrder();
    if (cachedOrder && cachedOrder.saleId === this.orderId) {
      this.order = cachedOrder;
      return;
    }
    if (this.orderId !== null) {
      this.receiptService.getOrderById(this.orderId).subscribe(result => {
        this.order = result;
      });
    }
  }
}
