import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recovery',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './recovery.html',
  styleUrl: './recovery.css'
})
export class recovery {
  email: string = '';

  constructor(private router: Router) {}

  onSubmit() {
    if (!this.email) {
      alert('Por favor ingresa tu correo electrónico');
      return;
    }

    // Aquí luego harás la llamada al backend (POST /api/usuario/recovery)
    alert('Se ha enviado un enlace de recuperación al correo proporcionado.');
    this.router.navigate(['/login']);
  }
}
