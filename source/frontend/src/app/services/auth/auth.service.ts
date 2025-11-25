import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  private http = inject(HttpClient);


  login(username: string, password: string) {
    const body = { "Username":username, "Password":password };
    return this.http.post(`/api/auth/login`, body);
  }
}
