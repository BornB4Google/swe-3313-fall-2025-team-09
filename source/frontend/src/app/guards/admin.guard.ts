import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

export const adminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.ensureCurrentUserLoaded().pipe(
    map(() => {
      const user = authService.currentUser();
      if (!user) {
        return router.createUrlTree(['/login']);
      }
      if (!user.isAdmin) {
        return router.createUrlTree(['/inventory']);
      }
      return true;
    })
  );
};
