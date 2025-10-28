import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  user = {
    nombre: '',
    correo: '',
    contrasena: '',
    //confirmarContrasena: '',
    tipoCuenta: 'customer'
  };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    /*if (this.user.contrasena !== this.user.confirmarContrasena) {
      alert('Las contraseñas no coinciden');
      return;
    }*/

    this.authService.register(this.user).subscribe({
      next: (res) => {
        alert('Usuario registrado con éxito');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error(err);
        alert('Error al registrar el usuario');
      }
    });
  }
}
