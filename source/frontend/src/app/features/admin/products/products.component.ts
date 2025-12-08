import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InventoryService } from '../../../services/inventory/inventory.service';
import { CommonModule } from '@angular/common';
import { InventoryItem, InventoryImage } from '../../../models/inventory.models';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css',
})
export class ProductsComponent {
  private inventoryService = inject(InventoryService);

  errorMessage: string | null = null;

  newProduct: InventoryItem = {
    itemId: 0,
    name: '',
    description: '',
    price: null,
    primaryPhotoUrl: '',
    category: '',
    isSold: false,
    images: [],
  };
  products: InventoryItem[] = [];
  submitNewProduct() {
    this.inventoryService.addInventoryItem(this.newProduct).subscribe({
      next: createdItem => {
        this.errorMessage = null;
        this.products.push(createdItem);
        this.newProduct = {
          itemId: 0,
          name: '',
          description: '',
          price: 0,
          primaryPhotoUrl: '',
          category: '',
          isSold: false,
          images: [],
        };
      },
      error: err => {
        console.error('Could not add item', err);
        const serverError = typeof err?.error === 'string' ? err.error : err?.error?.message;
        const fallback = 'Failed to add product. Please try again.';
        this.errorMessage = serverError?.trim()?.length ? serverError : fallback;
      },
    });
  }
  addImage() {
    const newImage: InventoryImage = { imageId: 0, imageUrl: '', displayOrder: 0 };
    this.newProduct.images.push(newImage);
  }
}
