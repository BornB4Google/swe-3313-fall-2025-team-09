import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryService } from '../../services/inventory/inventory.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { InventoryItem } from '../../models/inventory.models';
import { CartService } from '../../services/cart/cart.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-inventory',
  imports: [CommonModule],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent implements OnInit {
  inventoryService = inject(InventoryService);
  cartService = inject(CartService);
  private route = inject(ActivatedRoute);

  inventoryData = toSignal(this.inventoryService.getInventory(), {
    initialValue: [] as InventoryItem[],
  });

  itemsPerPage = 12;
  currentPage = 1;

  searchQuery = '';
  searchResults: InventoryItem[] = [];
  isSearching = false;

  onSearch() {
    if (!this.searchQuery.trim()) {
      this.clearSearch();
      return;
    }

    this.isSearching = true;
    this.inventoryService.searchInventory(this.searchQuery).subscribe({
      next: (results) => {
        this.searchResults = results;
        this.currentPage = 1;
      },
      error: (err) => {
        console.error('Search failed:', err);
        this.searchResults = [];
      }
    });
  }

  clearSearch() {
    this.searchQuery = '';
    this.searchResults = [];
    this.isSearching = false;
    this.currentPage = 1;
  }

  get displayedItems(): InventoryItem[] {
    if (this.isSearching) {
      return this.searchResults;
    }
    return this.inventoryData().filter(item => !item.isSold);
  }


  addToCart(item: InventoryItem) {
    if (this.cartService.isInCart(item.itemId)) {
      return;
    }
    this.cartService.addToCart(item.itemId);
  }

  get totalPages(): number {
    return Math.ceil(this.displayedItems.length / this.itemsPerPage);
  }
  get pageInventory() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.displayedItems.slice(start, end);
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

    this.route.queryParams.subscribe(params => {
      if (params['q']) {
        this.searchQuery = params['q'];
        this.onSearch();
      } else {
        this.clearSearch();
      }
    });
  }
}
