import { Component, inject, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, debounceTime, distinctUntilChanged, filter } from 'rxjs';
import { AuthService } from './services/auth/auth.service';
import { CartService } from './services/cart/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, FormsModule],
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

  showSearch = false;
  searchQuery = '';
  private searchInput$ = new Subject<string>();
  private isSearchNavigation = false;

  toggleSearch() {
    this.showSearch = !this.showSearch;
  }

  closeSearch() {
    this.showSearch = false;
    this.searchQuery = '';
    const currentUrl = this.router.url;
    if (!currentUrl.includes('shoppingCart') && !currentUrl.includes('checkout')) {
      this.router.navigate(['/inventory']);
    }
  }
  onSearch() {
    const trimmed = this.searchQuery.trim();
    if (trimmed) {
      this.isSearchNavigation = true;
      this.router.navigate(['/inventory'], { queryParams: { q: trimmed } });
    }
  }

  clearSearch() {
    this.searchQuery = '';
    this.onSearchInput('');
  }

  onSearchInput(value: string) {
    this.searchInput$.next(value);
  }

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
        if (this.isSearchNavigation) {
          this.isSearchNavigation = false;
        } else {
          this.showSearch = false;
        }
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
    this.searchInput$.pipe(debounceTime(100), distinctUntilChanged()).subscribe(query => {
      const trimmed = query?.trim() ?? '';
      this.isSearchNavigation = true;
      if (trimmed) {
        this.router.navigate(['/inventory'], { queryParams: { q: trimmed }, replaceUrl: true });
      } else {
        this.router.navigate(['/inventory'], { replaceUrl: true });
      }
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
  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}
