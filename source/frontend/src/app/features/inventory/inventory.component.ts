import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryService } from '../../services/inventory/inventory.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { InventoryItem } from '../../models/inventory.models';
import { CartService } from '../../services/cart/cart.service';

@Component({
  selector: 'app-inventory',
  imports: [CommonModule],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent implements OnInit {
  inventoryService = inject(InventoryService);

  inventoryData = toSignal(this.inventoryService.getInventory(), {
    initialValue: [] as InventoryItem[],
  });

  itemsPerPage = 12;
  currentPage = 1;
  cartService = inject(CartService);

  addToCart(item: InventoryItem) {
    if (this.cartService.isInCart(item.itemId)) {
      return;
    }
    this.cartService.addToCart(item.itemId);
  }

  get totalPages(): number {
    return Math.ceil(this.inventoryData().length / this.itemsPerPage);
  }
  get pageInventory() {
    const available = this.inventoryData().filter(item => !item.isSold);
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return available.slice(start, end);
  }
  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  selectedItem: InventoryItem | null = null;

  openItem(item: InventoryItem) {
    this.selectedItem = item;
  }

  closeOverlay() {
    this.selectedItem = null;
  }
  formatCompact(value: number) {
    return new Intl.NumberFormat('en-US', {
      notation: 'compact',
      compactDisplay: 'short',
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value);
  }
  ngOnInit() {
    this.cartService.loadCart();
  }
}
