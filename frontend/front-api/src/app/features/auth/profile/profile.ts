import { Component, OnInit } from '@angular/core';
import { Direccion } from '../../direcciones/direccion.model';
import { ProfileService } from './profile.service';
import { FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  metodosPago: any[] = [];

  metodoForm!: FormGroup;

  cargando = false;

  nuevaDireccion: Partial<Direccion> = {
    calle: '',
    ciudad: '',
    departamento: '',
    codigoPostal: '',
    pais: '',
    telefono: ''
  };

  nuevoMetodoPago = {
    nombreTitular: '',
    numeroTarjeta: '',
    mesExp: '',
    anioExp: '',
    cvv: ''
  };

  editIndex: number | null = null;

  constructor(private profileService: ProfileService, private fb: FormBuilder) {}

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
    this.crearFormularioMetodo();
    this.cargarMetodosPago();
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

  // Crear formulario reactivo
  crearFormularioMetodo() {
    this.metodoForm = this.fb.group({
      nombreTitular: ['', Validators.required],
      numeroTarjeta: ['', [Validators.required, Validators.minLength(12)]],
      mesExp: ['', [Validators.required, Validators.min(1), Validators.max(12)]],
      anioExp: ['', [Validators.required, Validators.min(2024)]],
      cvv: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(4)]],
    });
  }

  // Cargar métodos de pago
  cargarMetodosPago() {
    this.profileService.obtenerMetodosPago().subscribe({
      next: data => this.metodosPago = data,
      error: err => console.error('Error cargando métodos de pago', err)
    });
  }

  agregarMetodoPago() {

    if (!this.nuevoMetodoPago.numeroTarjeta || !this.nuevoMetodoPago.anioExp || !this.nuevoMetodoPago.mesExp) {
      alert("Completa número y fecha de expiración");
      return;
    }

    // Convertimos a formato DateOnly compatible
    //const fecha = `${this.nuevoMetodoPago.anioExp}-${this.nuevoMetodoPago.mesExp}-01`;

    const NumeroTarjeta = this.nuevoMetodoPago.numeroTarjeta;
    //const ultimos4 =  NumeroTarjeta.slice(-4);

    /*const NumeroTarjeta = this.nuevoMetodoPago.numeroTarjeta ?? '';
    if (!NumeroTarjeta || NumeroTarjeta.length < 4) {
      alert("Ingresa un número de tarjeta válido");
      return;
    }
    const ultimos4 = NumeroTarjeta.slice(-4);*/


    const year = this.nuevoMetodoPago.anioExp.toString().padStart(4, '20'); // ejemplo: "2027"
    const month = this.nuevoMetodoPago.mesExp.toString().padStart(2, '0'); // "02"
    const fecha = `${year}-${month}-01`; // formato correcto "2027-02-01"

    const metodoData = {
      tipo: 'tarjeta',
      numeroToken: NumeroTarjeta,
      banco: this.nuevoMetodoPago.nombreTitular || 'N/A',
      fechaExpiracion: fecha
    };

    this.profileService.agregarMetodoPago(metodoData).subscribe({
      next: () => {
        alert('Método agregado ');
        this.nuevoMetodoPago = { nombreTitular: '', numeroTarjeta: '', mesExp: '', anioExp: '', cvv: '' };
        this.cargarMetodosPago();
      },
      error: err => console.error('Error agregando método', err)
    });
    console.log(metodoData);
  }

  // Eliminar método de pago
  eliminarMetodoPago(id: number) {
    if (!confirm('¿Eliminar método de pago?')) return;

    this.profileService.eliminarMetodoPago(id).subscribe({
      next: () => {
        alert('Método eliminado ');
        this.cargarMetodosPago();
      },
      error: err => console.error('Error eliminando método', err)
    });
  }
}
