import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './carrito.html',
  styleUrls: ['./carrito.css']
})
export class carrito {
  isLogged = true; // Simulación de usuario loggeado
  cartItems = [
    { nombre: 'Auriculares inalámbricos', precio: 150, cantidad: 2 },
    { nombre: 'Smartwatch FitLife', precio: 80, cantidad: 1 },
    { nombre: 'Mochila UrbanTech', precio: 200, cantidad: 1 },
  ];

  get total() {
    return this.cartItems.reduce((acc, item) => acc + item.precio * item.cantidad, 0);
  }

  constructor(private router: Router) {}

  eliminarItem(index: number) {
    this.cartItems.splice(index, 1);
  }

  irAPagar() {
  alert(`Proceder a pagar: Total Q. ${this.total}`);
  }

  logout() {
  console.log('Logout simulado');
  this.router.navigate(['/home']);
  }

}
