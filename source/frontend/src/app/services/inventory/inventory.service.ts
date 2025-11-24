import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpParameterCodec } from '@angular/common/http';
import { inject } from '@angular/core';
import {Observable} from 'rxjs';
import {InventoryItem} from '../../models/inventory.models';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  private http = inject(HttpClient);


  constructor() { }

  getRandom(){
    return Math.random();
  }

  getInventory(): Observable<InventoryItem[]>{
    return this.http.get<InventoryItem[]>('/api/inventory');
  }

}
