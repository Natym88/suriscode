import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { articulo } from '../models/articulo.model';
import { API_URL } from './APIConfig';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArticuloService {

  ENDPOINTS = {
    GET_ALL: "articulos"
  }
  constructor(private http: HttpClient) { }

  getAllArticulos(): Observable<Array<articulo>> {
    return this.http.get<Array<articulo>>(API_URL + this.ENDPOINTS.GET_ALL);
  }
}
