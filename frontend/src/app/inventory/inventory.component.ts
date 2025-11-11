import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-inventory',
    imports: [
        RouterLink,
      CommonModule
    ],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})

export class InventoryComponent {
  // placeholder until database is connected
  inventory =[
    {name: 'name1', price: 0.00},
    {name: 'name2', price: 0.00},
    {name: 'name3', price: 0.00},
    {name: 'name4', price: 0.00},
    {name: 'name5', price: 0.00},
    {name: 'name6', price: 0.00},
    {name: 'name7', price: 0.00},
    {name: 'name8', price: 0.00},
    {name: 'name9', price: 0.00},
    {name: 'name10', price: 0.00},
    {name: 'name11', price: 0.00},
    {name: 'name12', price: 0.00},
    {name: 'name13', price: 0.00},
    {name: 'name14', price: 0.00},
    {name: 'name15', price: 0.00},
    {name: 'name16', price: 0.00},
    {name: 'name17', price: 0.00},
    {name: 'name18', price: 0.00},
    {name: 'name19', price: 0.00},
    {name: 'name20', price: 0.00},
    {name: 'name21', price: 0.00},
    {name: 'name22', price: 0.00},
    {name: 'name23', price: 0.00},
    {name: 'name24', price: 0.00}
  ];
  itemsPerPage = 12;
  currentPage = 1;

  addToCart(item: any) {
    console.log(`${item.name} added to cart`);
  }

  get totalPages(): number{
        return Math.ceil(this.inventory.length / this.itemsPerPage);
  }
  get pageInventory(){
    const start = (this.currentPage -1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.inventory.slice(start, end);
  }
  changePage(page: number){
    if (page >= 1 && page <= this.totalPages){
      this.currentPage = page;
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }
}
