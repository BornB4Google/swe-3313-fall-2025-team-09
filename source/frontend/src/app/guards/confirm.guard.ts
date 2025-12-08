import { CanActivateFn } from '@angular/router';

export const confirmGuard: CanActivateFn = (route, state) => {
  return true;

};
