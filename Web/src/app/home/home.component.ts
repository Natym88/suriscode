import { Component, OnInit } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {MatSelectChange, MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { vendedor } from '../models/vendedor.model';
import { VendedorService } from '../services/vendedor.service';
import { MatOptionModule } from '@angular/material/core';
import { NgFor, NgIf } from '@angular/common';
import {MatTableModule} from '@angular/material/table';
import { articulo } from '../models/articulo.model';
import { ArticuloService } from '../services/articulo.service';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { Pedido, pedido } from '../models/pedido.model';
import {MatButtonModule} from '@angular/material/button';
import { PedidoService } from '../services/pedido.service';
import { HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, NgFor, MatTableModule, MatCheckboxModule, NgIf, MatButtonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  vendedores: Array<vendedor> = [];
  articulos: Array<articulo> = [];
  displayedColumns: string[] = ['Código', 'Descripción', 'Precio', 'Agregar'];
  pedido: pedido = new Pedido();
  selected: string = "";

  constructor(private vendedorService: VendedorService, private articuloService: ArticuloService, private pedidoService: PedidoService){}
  ngOnInit(): void {
    this.getVendedores();
    this.getArticulos();
  }

  getVendedores(): void {
    this.vendedorService.getAllFromMocky().subscribe( v => {
      if(v.vendedores.length > 0){
        this.vendedores = v.vendedores;
      }
      // Por si falla la url de Mocky
      else{
        this.vendedorService.getAllFromAPI().subscribe( vApi => this.vendedores = vApi);
      }
    });
  }

  getArticulos(): void {
    this.articuloService.getAllArticulos().subscribe( a => this.articulos = a.filter(art => art.precio > 0));
  }

  update(checked: boolean, codigo: string): void {
    if(checked)
      this.pedido.articuloIds.push(codigo);
    else {
      this.pedido.articuloIds = this.pedido.articuloIds.filter( articulo => articulo != codigo);
    }
  }

  onSelected(event: MatSelectChange) {
    this.pedido.vendedorId = event.value;
  }

  pedir(event: Event) {
    event.preventDefault();
    
    this.pedidoService.generarPedido(this.pedido).subscribe(res => {
      if(typeof(res) == 'string') {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: res,
          showConfirmButton: true
        })
      }
    })
  }
}
