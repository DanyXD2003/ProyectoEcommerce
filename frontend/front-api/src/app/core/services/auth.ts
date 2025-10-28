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
      // Suponiendo que la URL base del backend está en environment.apiUrl
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
  }

