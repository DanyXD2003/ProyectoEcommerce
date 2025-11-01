// frontend/front-api/src/app/features/auth/login/login.ts
import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AlertModalComponent } from '../../../core/services/modal-alert/modal-alert'; // <-- ajusta ruta según tu estructura

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule, RouterLink, MatDialogModule]
})
export class Login {
  Correo = '';
  Contrasena = '';
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient,
    private dialog: MatDialog
  ) {}

  onSubmit() {
    console.log('onSubmit llamado', this.Correo, this.Contrasena);
    this.errorMessage = null;

    this.authService.login({ Correo: this.Correo, Contrasena: this.Contrasena }).subscribe({
      next: (response: any) => {
        console.log('Respuesta del backend:', response);

      // Guardar el token si existe
      if (response.Token || response.token) {
        const token = response.Token || response.token;
        localStorage.setItem('access_token', token);
      }

      localStorage.setItem('correoUsuario', this.Correo); 

      // Si viene un usuario, guárdalo
      if (response.usuario) {
        localStorage.setItem('usuario', JSON.stringify(response.usuario));
      }

      // Si NO viene usuario, crea un usuario básico
      else {
        localStorage.setItem('usuario', JSON.stringify({ nombre: this.Correo }));
}
        // Abrir modal de éxito
        const dialogRef = this.dialog.open(AlertModalComponent, {
          data: {
            title: 'Inicio de sesión exitoso',
            message: `Bienvenido`
          },
              width: '450px',        
              minWidth: '320px',   
              maxWidth: '90vw',      
              height: '150px',      
              panelClass: 'mat-mdc-dialog-container'
        });

        // Cuando el usuario cierre el modal, redirige
        dialogRef.afterClosed().subscribe(() => {
          this.router.navigate(['/home']).then(() => {
            window.location.reload(); 
          });
        });
      },
      error: (error: any) => {
        console.error('Error en el login:', error);
        this.errorMessage = 'Ocurrió un error de red o de servidor';

        // También podrías mostrar un modal de error
        this.dialog.open(AlertModalComponent, {
          data: {
            title: 'Error',
            message: 'Credenciales incorrectas o error del servidor.'
          },
          width: '350px'
        });
      }
    });
  }
}
