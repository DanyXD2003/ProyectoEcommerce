import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CarritoService, CarritoDto } from './carrito.service';
import { AuthService } from '../../core/services/auth';
import { ProfileService } from '../auth/profile/profile.service';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './carrito.html',
  styleUrls: ['./carrito.css']
})
export class Carrito implements OnInit {

  isLogged = false;
  carrito!: CarritoDto;

  // NUEVOS CAMPOS
  codigoDescuento = "";
  aplicando = false;
  pagando = false;

  direcciones: any[] = [];
  metodosPago: any[] = [];

  direccionSeleccionada = 0;
  metodoSeleccionado = 0;
  tipoPago = 'tarjeta'; // tarjeta o contraEntrega

  constructor(
    private carritoService: CarritoService,
    private authService: AuthService,
    private router: Router,
    private profileService: ProfileService   // agregado
  ) {}

  ngOnInit() {
    this.isLogged = this.authService.isLoggedIn();

    if (!this.isLogged) {
      this.router.navigate(['/login']);
      return;
    }

    this.cargarCarrito();
    this.cargarDatosCliente();  // agregado
  }

  cargarCarrito() {
    this.carritoService.obtenerCarrito().subscribe({
      next: data => {
        this.carrito = data as any;

        // Si el backend trae totales correctamente
        if (data.totalConDescuento !== undefined) {
          (this.carrito as any).total = data.totalConDescuento;
        } else {
          // Fallback si no viene desde backend
          (this.carrito as any).total = data.detalles.reduce((t, d) => t + d.subtotal, 0);
        }
      },
      error: err => {
        console.error("Error cargando carrito", err);
      }
    });
  }



  // NUEVO: cargar direcciones y métodos de pago
  cargarDatosCliente() {
    const correo = localStorage.getItem('correoUsuario');
    if (!correo) return;

    this.profileService.buscarPorCorreo(correo).subscribe({
      next: data => {
        this.direcciones = data.direcciones || [];

        this.profileService.obtenerMetodosPago().subscribe({
          next: m => this.metodosPago = m
        });
      },
      error: err => console.log("Error cargando datos de cliente", err)
    });
  }

  // NUEVO: aplicar descuento
  aplicarDescuento() {
    if (!this.codigoDescuento.trim()) {
      alert("Ingresa un código válido");
      return;
    }

    this.aplicando = true;
    this.carritoService.aplicarDescuento(this.codigoDescuento).subscribe({
      next: c => {
        this.carrito = c;
        this.codigoDescuento = "";
        this.aplicando = false;
        alert("Descuento aplicado ✔");
      },
      error: e => {
        this.aplicando = false;
        alert("Código inválido o expirado");
      }
    });
  }

  eliminarItem(productoId: number) {
    this.carritoService.eliminarProducto(productoId).subscribe(() => {
      this.cargarCarrito();
    });
  }

  vaciar() {
    this.carritoService.vaciarCarrito().subscribe(() => {
      this.cargarCarrito();
    });
  }

  // NUEVO: activar pantalla de pago
  irAPagar() {
    this.pagando = true;
  }

  // NUEVO: confirmar pedido
  confirmarPedido() {
    if (!this.direccionSeleccionada || !this.metodoSeleccionado) {
      alert("Debes seleccionar dirección y método de pago");
      return;
    }

    this.carritoService.crearPedido(
      this.direccionSeleccionada,
      this.metodoSeleccionado,
      this.tipoPago
    ).subscribe({
      next: () => {
        alert("Pedido creado");
        this.router.navigate(['/profile']);
      },
      error: err => {
        console.log(err);
        alert("Error creando pedido");
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
