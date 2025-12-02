import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryImage } from '../../../models/inventory.models';

@Component({
  selector: 'app-inventory-carousel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-carousel.component.html',
  styleUrls: ['./inventory-carousel.component.css']
})
export class InventoryCarouselComponent {

  //sets a required parameter - an array of InventoryImages
  @Input({ required: true }) images: InventoryImage[] = [];

  //signal so re-rendered automatically when changed
  currentIndex = signal(0);

  //use mod to wrap around to start of array
  next(): void {
    const n = this.images.length;
    if (n === 0) return;
    this.currentIndex.update(i => (i + 1) % n);
  }

  prev(): void {
    const n = this.images.length;
    if (n === 0) return;
    this.currentIndex.update(i => (i - 1 + n) % n);
  }




}
