import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AlertModalComponent } from '../../../core/services/modal-alert/modal-alert';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, MatDialogModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  user = {
    nombre: '',
    apellido: '',
    correo: '',
    contrasena: '',
    //confirmarContrasena: '',
    tipoCuenta: 'cliente'
  };

  constructor(private authService: AuthService, private router: Router, private dialog: MatDialog) {}

  onSubmit() {
    this.authService.register(this.user).subscribe({
      next: (res) => {

        // Abrir modal con el nombre que viene de la API
        const dialogRef = this.dialog.open(AlertModalComponent, {
          data: {
            title: 'Usuario registrado con éxito',
            message: `Bienvenido, ${res.nombre || this.user.nombre}`
          },
          width: '450px',
          minWidth: '320px',
          maxWidth: '90vw',
          height: '150px',
          panelClass: 'mat-mdc-dialog-container'
        });

        // Cuando cierre el modal → redirige
        dialogRef.afterClosed().subscribe(() => {
          this.router.navigate(['/login']);
        });
      },
      error: (err) => {
        console.error(err);

        // Modal de error (mejor que alert)
        this.dialog.open(AlertModalComponent, {
          data: {
            title: 'Error',
            message: 'Hubo un problema al registrar el usuario'
          },
          width: '350px'
        });
      }
    });
  }
}
