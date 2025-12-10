import { Component, Input, signal, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryItem } from '../../../models/inventory.models';

@Component({
  selector: 'app-inventory-carousel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-carousel.component.html',
  styleUrls: ['./inventory-carousel.component.css'],
})
export class InventoryCarouselComponent implements OnChanges {
  // Sets a required parameter - an InventoryItem
  @Input({ required: true }) item!: InventoryItem;

  imageUrls: string[] = [];

  // Signal so re-rendered automatically when changed
  currentIndex = signal(0);

  // Use mod to wrap around to start of array
  next(): void {
    const n = this.imageUrls.length;
    if (n === 0) return;
    this.currentIndex.update(i => (i + 1) % n);
  }

  prev(): void {
    const n = this.imageUrls.length;
    if (n === 0) return;
    this.currentIndex.update(i => (i - 1 + n) % n);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['item'] && this.item) {
      this.changeUrls();
    }
  }

  private changeUrls(): void {
    // Clear then add new images so doesn't keep old ones
    this.imageUrls = [];
    // Set to 0
    this.currentIndex.update(i => i - i);
    // PrimaryPhotoUrl always index 0 in the array imageUrls
    this.imageUrls.push(this.item.primaryPhotoUrl);
    // Add rest of urls from the array images as described in inventory.models.ts
    for (const image of this.item.images) {
      this.imageUrls.push(image.imageUrl);
    }
  }
}
