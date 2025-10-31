import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  /*private mockUser = {
    email: 'usuario@ejemplo.com',
    password: '123456',
    token: 'mock-jwt-token'
  };*/
  constructor(private http: HttpClient) {}

  login(credentials: { Correo: string; Contrasena: string }): Observable<any> {
      const url = `http://localhost:5000/api/usuario/login`; 

      return this.http.post<any>(url, credentials);
    }

  register(userData: any): Observable<any> {
    const url = `http://localhost:5000/api/usuario/registrar`; 
    return this.http.post<any>(url, userData);
  }

  getToken(): string | null {
      return localStorage.getItem('access_token');
    }
  // Saber si el usuario está logueado
  isLoggedIn(): boolean {
    return !!localStorage.getItem('access_token');
  }

  // Obtener información del usuario
  getUser(): any {
    const data = localStorage.getItem('usuario');
    return data ? JSON.parse(data) : null;
  }

  // Cerrar sesión
  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('usuario');
  }
}