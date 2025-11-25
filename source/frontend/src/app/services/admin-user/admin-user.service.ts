import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { User } from '../../models/user.models';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private http = inject(HttpClient);


  constructor() { }

  getUsers(){
    return this.http.get<User[]>('/api/users');
  }

  setUserRole(userId: number, isAdmin: boolean): Observable<any> {
    const body = { isAdmin: isAdmin };
    return this.http.put(`/api/users/${userId}/role`, body);
  }



}
