import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CarritoService, CarritoDto } from './carrito.service';
import { AuthService } from '../../core/services/auth';

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

  constructor(
    private carritoService: CarritoService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.isLogged = this.authService.isLoggedIn();

    if (!this.isLogged) {
      this.router.navigate(['/login']);
      return;
    }

    this.cargarCarrito();
  }

  cargarCarrito() {
    this.carritoService.obtenerCarrito().subscribe({
      next: data => {
        this.carrito = data;
      },
      error: err => {
        console.error("Error cargando carrito", err);
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

  irAPagar() {
    alert("Aquí iría la página de pago real");
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
