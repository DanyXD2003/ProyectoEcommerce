import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {

  private baseUrl = 'https://proyectoecommerceback.onrender.com/api/Categoria';

  constructor(private http: HttpClient) {}

  obtenerTodas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/obtenerTodas`);
  }
}
