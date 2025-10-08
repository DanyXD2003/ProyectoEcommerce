import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private mockUser = {
    email: 'usuario@ejemplo.com',
    password: '123456',
    token: 'mock-jwt-token'
  };

  login(credentials: { email: string; password: string }): Observable<any> {
    if (
      credentials.email === this.mockUser.email &&
      credentials.password === this.mockUser.password
    ) {
      return of({ token: this.mockUser.token }).pipe(delay(1000));
    } else {
      return of({ error: 'Credenciales inválidas' }).pipe(delay(1000));
    }
  }
}
