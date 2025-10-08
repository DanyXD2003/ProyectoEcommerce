import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule]
})
export class Login {
  email = '';
  password = '';
  errorMessage: string | null = null;

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (response: { token: any; error: string | null; }) => {
        if (response.token) {
          console.log('Login exitoso', response);
          // Aquí puedes redirigir al usuario a otra página, como el perfil
        } else {
          this.errorMessage = response.error;
        }
      },
      error: (error: any) => {
        console.error('Error en el login', error);
        this.errorMessage = 'Ocurrió un error. Intenta nuevamente.';
      }
    });
  }
}
