import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-signup',
  imports: [RouterLink, FormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SignupComponent {
  authService = inject(AuthService);
  router = inject(Router);

  firstName = '';
  lastName = '';
  username = '';
  email = '';
  password = '';

  errorMessage: string | null = null;

  register() {
    console.log('register clicked');
    this.authService
      .register(this.username, this.password, this.email, this.firstName, this.lastName)
      .subscribe({
        next: response => {
          console.log('register successful!');
          console.log('Server Response:', response);
          this.router.navigate(['/inventory']);
        },
        error: err => {
          console.log('Login failed!');
          console.log('Server Response:', err);
          const serverError = typeof err?.error === 'string' ? err.error : err?.error?.message;
          const fallback = 'error: validation failed </3';
          this.errorMessage = serverError?.trim()?.length ? serverError : fallback;
          this.password = '';
        },
      });
  }
}
