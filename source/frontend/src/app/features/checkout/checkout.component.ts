import { Component, NgModule } from '@angular/core';
import { RouterLink } from "@angular/router";
import { FormsModule } from '@angular/forms';
import { ShippingService } from '../../services/shipping/shipping.service';
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
  formatPhone(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    event.target.value = value;
  }
  formatCC(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{4})(\d{4})(\d{4})(\d{4})/, '$1 $2 $3 $4');
    event.target.value = value;
  }
  formatNumOnly(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');

    event.target.value = value;
  }
  formatDate(event:any) {
    let value = event.target.value.replace(/[^0-9]/g, '');
    value = value.replace(/(\d{2})(\d{4})/, '$1/$2');
    event.target.value = value;
  }

}

