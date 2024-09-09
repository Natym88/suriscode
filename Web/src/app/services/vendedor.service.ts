import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { vendedor, vendedorMocky } from '../models/vendedor.model';
import { HttpClient } from '@angular/common/http';
import { API_URL, VENDEDORES_URL } from './APIConfig';

@Injectable({
  providedIn: 'root'
})
export class VendedorService {
  
  ENDPOINTS = {
    GET_ALL: "vendedores"
  }

  constructor(private http: HttpClient) { }

  getAllFromMocky (): Observable<vendedorMocky> {
    return this.http.get<vendedorMocky>(VENDEDORES_URL);
  }

  getAllFromAPI(): Observable<Array<vendedor>> {
    return this.http.get<Array<vendedor>>(API_URL + this.ENDPOINTS.GET_ALL);
  }
}
