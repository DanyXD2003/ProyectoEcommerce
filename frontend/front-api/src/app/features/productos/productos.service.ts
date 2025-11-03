import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductoDto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number;
  categoriaNombre: string;
  activo: boolean;
  cantidad?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

  private baseUrl = 'https://proyectoecommerceback.onrender.com/api';

  constructor(private http: HttpClient) {}

  obtenerTodos(): Observable<ProductoDto[]> {
    return this.http.get<ProductoDto[]>(`${this.baseUrl}/Producto/obtenerTodos`);
  }

  obtenerPorCategoria(categoriaId: number): Observable<ProductoDto[]> {
    return this.http.get<ProductoDto[]>(`${this.baseUrl}/Producto/obtenerPorCategoria/${categoriaId}`);
  }
}
