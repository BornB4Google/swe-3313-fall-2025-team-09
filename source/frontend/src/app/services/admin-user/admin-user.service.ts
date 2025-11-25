import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { User } from '../../models/user.models';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private http = inject(HttpClient);


  constructor() { }

  getUsers(){
    return this.http.get<User[]>('/api/users');
  }

}
