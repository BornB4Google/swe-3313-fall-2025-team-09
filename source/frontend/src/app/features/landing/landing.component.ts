import { Component } from '@angular/core';
import { RouterLink } from '@angular/router'; // needed for routerLink

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
})
export class LandingComponent {}
