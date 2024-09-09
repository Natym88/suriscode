import { Injectable } from '@angular/core';
import { pedido } from '../models/pedido.model';
import { HttpClient } from '@angular/common/http';
import { API_URL } from './APIConfig';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  ENDPOINTS = {
    CREATE: "pedido"
  }
  constructor(private http: HttpClient) { }

  generarPedido(pedido: pedido): Observable<any> {
    return this.http.post<pedido>(API_URL + this.ENDPOINTS.CREATE, pedido).pipe();
  }
}
