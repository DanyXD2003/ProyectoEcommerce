import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
//import { jwtDecode } from "../../../../node_modules/jwt-decode/build/esm/index";



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

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      console.log(decoded);

      // Detecta el claim largo de .NET
      const roleClaim = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      return roleClaim || null;

    } catch (e) {
      console.error("Error decodificando token:", e);
      return null;
    }
  }


  login(credentials: { Correo: string; Contrasena: string }): Observable<any> {
      const url = `https://proyectoecommerceback.onrender.com/api/usuario/login`; 

      return this.http.post<any>(url, credentials);
    }

  register(userData: any): Observable<any> {
    const url = `https://proyectoecommerceback.onrender.com/api/usuario/registrar`; 
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