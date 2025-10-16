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
  email = '';
  password = '';
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router, private http: HttpClient) {}

  onSubmit() {
    console.log('onSubmit llamado', this.email, this.password);
    this.errorMessage = null;

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (response: { token?: string; error?: string }) => {
        if (response.token) {
          // Aquí guardas el token porque la respuesta fue exitosa
          localStorage.setItem('access_token', response.token);
          console.log('Login exitoso', response);
          // Luego puedes redirigir a la página que quieras
          this.router.navigate(['/dashboard']);
        } else {
          // Si no hay token pero viene un error, muéstralo
          this.errorMessage = response.error || 'No se recibió respuesta válida del servidor';
        }
      },
      error: (error: any) => {
        console.error('Error en el login', error);
        this.errorMessage = 'Ocurrió un error de red o de servidor';
      }
    });
  }
}
