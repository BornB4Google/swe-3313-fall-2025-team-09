import { Component } from '@angular/core';
import { CurrencyPipe, NgForOf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ShippingService } from '../shipping.service';

@Component({
  selector: 'app-receipt',
  imports: [
    CurrencyPipe,
    NgForOf,
    RouterLink
  ],
  templateUrl: './receipt.component.html',
  styleUrl: './receipt.component.css'
})
export class ReceiptComponent {

  shipping: any = {};

  receiptItems=[
    {name: 'name1' ,price: 0.00},
    {name: 'name1', price: 0.00},
    {name: 'name1', price: 0.00}
  ];
  constructor(private shipService: ShippingService) {}

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
  }

}
