import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { OrderDetailDto } from '../../models/order.models';

@Component({
  selector: 'app-receipt-modal',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './receipt-modal.component.html',
  styleUrl: './receipt-modal.component.css',
})
export class ReceiptModalComponent {
  @Input() receipt: OrderDetailDto | null = null;
  @Output() close = new EventEmitter<void>();

  onOverlayClick(): void {
    this.close.emit();
  }

  onCloseClick(): void {
    this.close.emit();
  }

  stopPropagation(event: Event): void {
    event.stopPropagation();
  }
}
