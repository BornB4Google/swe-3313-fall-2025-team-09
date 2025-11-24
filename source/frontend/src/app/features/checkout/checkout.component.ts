import { Component, NgModule } from '@angular/core';
import { RouterLink } from "@angular/router";
import { FormsModule } from '@angular/forms';
import { ShippingService } from '../shipping.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})




export class CheckoutComponent {

  checkoutData = {
    name: '',
    address1: '',
    address2: '',
    city: '',
    state: '',
    zip: ''
  };
  constructor(
    private shipService: ShippingService,
    private router: Router
  ) {}


  confirmOrder() {
    this.shipService.shippingInfo = this.checkoutData;
    this.router.navigate(['/confirm']);
  }


}
