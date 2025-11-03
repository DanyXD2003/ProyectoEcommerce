import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Direccion } from '../../direcciones/direccion.model';

export interface UsuarioResponse {
  id: number;
  nombre: string;
  apellido: string;
  correo: string;
  pedidos: any[];
  direcciones: Direccion[];
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('access_token');
    console.log(token);
    return {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
  }

  // Obtener info completa del usuario por correo
  buscarPorCorreo(correo: string): Observable<UsuarioResponse> {
    return this.http.get<UsuarioResponse>(
      `${this.apiUrl}/usuario/GetInfoByCorreo?correo=${correo}`
    );
  }

  // Crear
  agregarDireccion(direccion: Direccion): Observable<Direccion> {
    return this.http.post<Direccion>(
      `${this.apiUrl}/Direccion/crearDireccion`,
      direccion,
      this.getAuthHeaders()
    );
  }

  // Actualizar
  actualizarDireccion(direccion: Direccion): Observable<Direccion> {
    return this.http.put<Direccion>(
      `${this.apiUrl}/direccion/actualizar/${direccion.id}`,
      direccion,
      this.getAuthHeaders()
    );
  }

  // Eliminar
  eliminarDireccion(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/direccion/eliminar/${id}`,
      this.getAuthHeaders()
    );
  }

  // MÉTODOS DE PAGO (NUEVO)

  // Obtener métodos del usuario autenticado
  obtenerMetodosPago(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiUrl}/MetodoPago/obtenerMisMetodos`,
      this.getAuthHeaders()
    );
  }

  // Agregar método de pago
  agregarMetodoPago(metodo: any): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/MetodoPago/agregarMetodo`,
      metodo,
      this.getAuthHeaders()
    );
  }

  // Eliminar método de pago
  eliminarMetodoPago(metodoId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/MetodoPago/eliminarMetodo/${metodoId}`,
      this.getAuthHeaders()
    );
  }
}
