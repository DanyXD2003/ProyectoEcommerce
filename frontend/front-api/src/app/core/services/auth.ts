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

  login(credentials: { email: string; password: string }): Observable<any> {
      // Suponiendo que la URL base del backend está en environment.apiUrl
      const url = `http://localhost:5000/api/usuario/login`;  // ajusta la ruta de tu API real

      return this.http.post<any>(url, credentials);
    }
    getToken(): string | null {
      return localStorage.getItem('access_token');
    }
  }

