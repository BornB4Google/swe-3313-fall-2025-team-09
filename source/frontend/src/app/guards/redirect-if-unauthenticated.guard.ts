import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

export const redirectIfUnauthenticatedGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.ensureCurrentUserLoaded().pipe(
    map(() => (!authService.isLoggedIn() ? router.createUrlTree(['/login']) : true))
  );
};
