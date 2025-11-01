import { Component, OnInit } from '@angular/core';
import { Direccion } from '../../direcciones/direccion.model';
import { ProfileService } from './profile.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { take } from 'rxjs/operators';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
  imports: [FormsModule, HttpClientModule, CommonModule, RouterLink]
})
export class ProfileComponent implements OnInit {

  user = {
    id: 0,
    nombre: '',
    apellido: '',
    correo: ''
  };

  direcciones: Direccion[] = [];
  pedidos: any[] = [];

  nuevaDireccion: Partial<Direccion> = {
    calle: '',
    ciudad: '',
    departamento: '',
    codigoPostal: '',
    pais: '',
    telefono: ''
  };

  editIndex: number | null = null;

  constructor(private profileService: ProfileService) {}

  ngOnInit(): void {
    const correo = localStorage.getItem('correoUsuario');

    if (!correo) {
      console.error('⚠ No se encontró el correo del usuario en localStorage.');
      return;
    }

    this.profileService.buscarPorCorreo(correo)
      .pipe(take(1))
      .subscribe({
        next: data => {
          this.user = {
            id: data.id,
            nombre: data.nombre,
            apellido: data.apellido,
            correo: data.correo
          };

          this.direcciones = data.direcciones || [];
          this.pedidos = data.pedidos || [];
        },
        error: err => console.error('Error cargando usuario', err)
      });
  }

  agregarDireccion() {
    // Validación mínima
    if (!this.nuevaDireccion.calle || !this.nuevaDireccion.ciudad) {
      alert('Completa al menos calle y ciudad');
      return;
    }

    const direccion: Direccion = {
      id: this.editIndex !== null ? this.direcciones[this.editIndex].id : 0,
      usuarioId: this.user.id,
      calle: this.nuevaDireccion.calle!,
      ciudad: this.nuevaDireccion.ciudad!,
      departamento: this.nuevaDireccion.departamento || '',
      codigoPostal: this.nuevaDireccion.codigoPostal || '',
      pais: this.nuevaDireccion.pais || '',
      telefono: this.nuevaDireccion.telefono || ''
    };

    // EDITAR
    if (this.editIndex !== null) {
      this.profileService.actualizarDireccion(direccion)
        .pipe(take(1))
        .subscribe({
          next: () => {
            this.direcciones[this.editIndex!] = direccion;
            alert('Dirección actualizada');
            this.resetForm();
          },
          error: err => console.error('Error al actualizar', err)
        });
    }

    // AGREGAR
    else {
      this.profileService.agregarDireccion(direccion)
        .pipe(take(1))
        .subscribe({
          next: resp => {
            this.direcciones.push(resp);
            alert('Dirección agregada');
            this.resetForm();
          },
          error: err => console.error('Error al agregar', err)
        });
    }
  }

  editarDireccion(i: number) {
    this.editIndex = i;
    this.nuevaDireccion = { ...this.direcciones[i] };
  }

  eliminarDireccion(i: number) {
    const id = this.direcciones[i].id;

    if (!confirm('¿Seguro que deseas eliminar esta dirección?')) return;

    this.profileService.eliminarDireccion(id)
      .pipe(take(1))
      .subscribe({
        next: () => {
          this.direcciones.splice(i, 1);
        },
        error: err => console.error('Error al eliminar', err)
      });
  }

  resetForm() {
    this.editIndex = null;
    this.nuevaDireccion = {
      calle: '',
      ciudad: '',
      departamento: '',
      codigoPostal: '',
      pais: '',
      telefono: ''
    };
  }
}
