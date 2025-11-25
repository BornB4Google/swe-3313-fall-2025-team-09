import {computed, inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {User} from '../../models/user.models';
import {catchError, Observable, of, tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() {
    this.refreshCurrentUser();
  }

  private http = inject(HttpClient);
  private readonly currentUserSignal = signal<User | null>(null);
  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isLoggedIn = computed(() => this.currentUserSignal() !== null);


  login(username: string, password: string) {
    const body = { "Username":username, "Password":password };
    return this.http.post(`/api/auth/login`, body).pipe(
      tap(() => this.refreshCurrentUser())
    );
  }

  refreshCurrentUser(): void {
    this.loadCurrentUser().subscribe();
  }

  private loadCurrentUser(): Observable<User | null> {
    return this.http.get<User>('/api/users/me').pipe(
      tap((user) => {
        this.currentUserSignal.set(user);
        console.log('current user set! ' + user.username);
        console.log('logged in: ' + this.isLoggedIn())
      }
      ),
      catchError((err) => {
        this.currentUserSignal.set(null);
        console.log('current user not set!' + err.message)
        return of(null);
      })
    );
  }
}
