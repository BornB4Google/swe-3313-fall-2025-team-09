import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [CommonModule, RouterLink, FormsModule],
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  authService = inject(AuthService);
  router = inject(Router);
  username = '';
  password = '';
  errorMessage: string | null = null;

  login() {
    // console.log('username:' +this.username, ' password:' + this.password);
    this.errorMessage = null;
    this.authService.login(this.username, this.password).subscribe({
      // This block runs if the request is SUCCESSFUL (status 2xx)
      next: response => {
        console.log('Login Successful!');
        console.log('Server Response:', response);
        this.router.navigate(['/inventory']);
      },
      error: err => {
        console.log('Login failed!');
        console.log('Server Response:', err);
        const serverError = typeof err?.error === 'string' ? err.error : err?.error?.message;
        const fallback = 'Invalid username or password. Please try again.';
        this.errorMessage = serverError?.trim()?.length ? serverError : fallback;
        this.password = '';
      },
    });
  }
}
