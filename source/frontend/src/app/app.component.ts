import { Component, inject, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs';
import { AuthService } from './services/auth/auth.service';
import { CartService } from './services/cart/cart.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  authService = inject(AuthService);
  cartService = inject(CartService);

  isLanding = false;
  isLanding2 = false;
  isLogin = false;
  isSignup = false;
  cartCount = 0;

  private router = inject(Router);

  constructor() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const url = event.urlAfterRedirects;
        this.isLanding = url === '/landing';
        this.isLanding2 = url === '/';
        this.isLogin = url === '/login';
        this.isSignup = url === '/signup';
        console.log(
          'URL:',
          url,
          'Landing:',
          this.isLanding,
          'Login:',
          this.isLogin,
          'Signup:',
          this.isSignup
        );
      });
  }

  ngOnInit() {
    this.cartService.loadCart();
    this.cartService.cartItems$.subscribe(cart => {
      this.cartCount = cart.items.length;
    });
  }

  logout() {
    this.authService.logout().subscribe({
      // This block runs if the request is SUCCESSFUL (status 2xx)
      next: response => {
        console.log('logged out');
        console.log('Server Response:', response);
        this.router.navigate(['/']);
      },
      error: err => {
        console.log('logout failed!');
        console.log('Server Response:', err);
      },
    });
  }
}
