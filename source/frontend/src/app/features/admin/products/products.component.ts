import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InventoryService } from '../../../services/inventory/inventory.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {

    newProduct: any = {
      itemId: 0,
      name: '',
      description: '',
      price: 0,
      primaryPhotoUrl: '',
      category: '',
      isSold: false,
      images: []
    };
    products: any[] = [];



  constructor(private inventoryService: InventoryService) {}
  submitNewProduct() {
      this.inventoryService.addInventoryItem(this.newProduct).subscribe({
        next:(createdItem) => {
          this.products.push(createdItem);
          this.newProduct = {
            itemId: 0,
            name: '',
            description: '',
            price: 0,
            primaryPhotoUrl: '',
            category: '',
            isSold: false,
            images: []
          };
        },
        error: (err) => console.error("Could not add item", err)
      });
  }
  addImage(){
      this.newProduct.images.push({ imageId: 0, imageUrl: '', })
  }



}
