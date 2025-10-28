// frontend/front-api/src/app/features/auth/login/login.ts
import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule, RouterLink]
})
export class Login {
  Correo = '';
  Contrasena = '';
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient
  ) {}

  onSubmit() {
    console.log('onSubmit llamado', this.Correo, this.Contrasena);
    this.errorMessage = null;

    this.authService.login({ Correo: this.Correo, Contrasena: this.Contrasena }).subscribe({
      next: (response: any) => {
        console.log(' Respuesta del backend:', response);

        // Si el backend devuelve un token (en caso de que lo agregues después)
        if (response.token) {
          localStorage.setItem('access_token', response.token);
          console.log('Login exitoso con token:', response.token);
          this.router.navigate(['/']);
        } else {
          // Si no hay token, imprime toda la data que vino del backend
          console.log('Datos del usuario:', response);
          localStorage.setItem('usuario', JSON.stringify(response));

          // También puedes redirigir al dashboard si el login fue exitoso
          this.router.navigate(['/dashboard']);
        }
      },
      error: (error: any) => {
        console.error('Error en el login:', error);
        this.errorMessage = 'Ocurrió un error de red o de servidor';
      }
    });
  }
}
