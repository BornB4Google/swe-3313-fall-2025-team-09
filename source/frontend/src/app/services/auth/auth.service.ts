import {computed, inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {User} from '../../models/user.models';
import {catchError, Observable, of, switchMap, tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() {
    this.refreshCurrentUser();
  }

  private http = inject(HttpClient);
  private readonly currentUserSignal = signal<User | null>(null);
  private hasFetchedCurrentUser = false;
  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isLoggedIn = computed(() => this.currentUserSignal() !== null);


  login(username: string, password: string) {
    const body = { "Username":username, "Password":password };
    return this.http.post(`/api/auth/login`, body).pipe(
      tap(() => this.refreshCurrentUser(true))
    );
  }

  register(username: string, password: string, email:string, firstName: string, lastName:string){
    const body = { "username":username, "password":password, "email":email, "firstName":firstName, "lastName":lastName };
    return this.http.post(`/api/auth/register`, body).pipe(
      switchMap(() => this.login(username, password))
    );
  }

  logout() {
    console.log('logout')
    const body = {  };
    return this.http.post(`/api/auth/logout`, body).pipe(
      tap(() => this.refreshCurrentUser(true))
    );
  }

  refreshCurrentUser(forceReload = false): void {
    if (forceReload) {
      this.hasFetchedCurrentUser = false;
    }

    this.ensureCurrentUserLoaded().subscribe();
  }

  ensureCurrentUserLoaded(): Observable<User | null> {
    if (this.hasFetchedCurrentUser) {
      return of(this.currentUserSignal());
    }

    return this.loadCurrentUser();
  }

  private loadCurrentUser(): Observable<User | null> {
    return this.http.get<User>('/api/users/me').pipe(
      tap((user) => {
        this.currentUserSignal.set(user);
        this.hasFetchedCurrentUser = true;
        console.log('current user set! ' + user.username);
        console.log('logged in: ' + this.isLoggedIn())
      }
      ),
      catchError((err) => {
        this.currentUserSignal.set(null);
        this.hasFetchedCurrentUser = true;
        console.log('current user not set!' + err.message)
        return of(null);
      })
    );
  }
}
