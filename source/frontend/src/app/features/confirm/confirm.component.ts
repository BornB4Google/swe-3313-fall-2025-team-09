import { Component } from '@angular/core';
import { CurrencyPipe, NgForOf} from "@angular/common";
import { RouterLink } from "@angular/router";
import { ShippingService } from '../../services/shipping/shipping.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-confirm',
    imports: [
        CurrencyPipe,
        NgForOf,
        RouterLink
    ],
  templateUrl: './confirm.component.html',
  styleUrl: './confirm.component.css'
})


export class ConfirmComponent {

  shipping: any = {};

  confirmOrder = [
    { name: 'name1', price: 0.00 },
    { name: 'name1', price: 0.00 },
    { name: 'name1', price: 0.00 }
  ];

  constructor(private shipService: ShippingService) {}

  ngOnInit() {
    this.shipping = this.shipService.shippingInfo;
  }

}
