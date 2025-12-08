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
  @Output() closeModal = new EventEmitter<void>();

  onOverlayClick(): void {
    this.closeModal.emit();
  }

  onCloseClick(): void {
    this.closeModal.emit();
  }

  stopPropagation(event: Event): void {
    event.stopPropagation();
  }
}
