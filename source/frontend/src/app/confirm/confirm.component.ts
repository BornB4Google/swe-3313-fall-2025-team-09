import { Component } from '@angular/core';
import {CurrencyPipe, NgForOf} from "@angular/common";
import {RouterLink} from "@angular/router";

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

}
