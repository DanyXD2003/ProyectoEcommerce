import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class ProfileComponent {

  // DATOS SIMULADOS — luego vendrán del backend
  user = {
    nombre: 'Juan',
    apellido: 'Pérez',
    email: 'juanperez@example.com'
  };

  direccion = {
    ciudad: '',
    detalle: ''
  };

  pedidos = [
    { id: 1, fecha: '2025-01-03', total: 150, estado: 'Entregado' },
    { id: 2, fecha: '2025-01-10', total: 80, estado: 'En camino' },
    { id: 3, fecha: '2025-02-01', total: 200, estado: 'Pendiente' },
  ];

  guardarDireccion() {
    console.log('Dirección guardada:', this.direccion);
    alert('Dirección guardada correctamente (mock)');
  }

}
