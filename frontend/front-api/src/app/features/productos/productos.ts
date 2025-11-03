import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductoService, ProductoDto } from './productos.service';
import { AuthService } from '../../core/services/auth';
import { CarritoService } from '../carrito/carrito.service';


@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, RouterLink],
  templateUrl: './productos.html',
  styleUrls: ['./productos.css'],
})
export class Productos implements OnInit {

  categorias = [
    { id: 1, nombre: 'Electrónica' },
    { id: 2, nombre: 'Ropa' },
    { id: 3, nombre: 'Hogar' }
  ];

  isLogged = false;
  userName = '';

  productos: ProductoDto[] = [];
  categoriaSeleccionada: number | null = null;

  constructor(
    private productoService: ProductoService, 
    private authService: AuthService, 
    private router: Router,
    private carritoService: CarritoService
  ) {}

  ngOnInit() {
    this.cargarTodos();

    //Saber si el usuario esta loggeado
    console.log("Token desde home:", localStorage.getItem('access_token'));

    this.isLogged = this.authService.isLoggedIn();
    console.log("¿Está logueado?:", this.isLogged);

    const user = this.authService.getUser();
    console.log("Usuario recuperado:", user);

    this.userName = user ? (user.nombre || user.correo || 'Usuario') : '';
  }
  

  cargarTodos() {
    this.categoriaSeleccionada = 0;
    this.productoService.obtenerTodos()
      .subscribe(data => {
        //this.productos = data;
        this.productos = data.map(p => ({ ...p, cantidad: 0 })); // ✅ agregar cantidad al front
      });
  }

  filtrarPorCategoria(catId: number) {
    this.categoriaSeleccionada = catId;
    this.productoService.obtenerPorCategoria(catId)
      .subscribe(data => {
        //this.productos = data;
        this.productos = data.map(p => ({ ...p, cantidad: 0 })); // ✅ lo mismo al filtrar
      });
  }

  logout() {
    this.authService.logout();
    this.isLogged = false;
    this.userName = '';
    this.router.navigate(['/home']);
  } 

  agregarAlCarrito(productoId: number) {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.carritoService.agregarProducto(productoId, 1).subscribe({
      next: () => this.router.navigate(['/carrito']),
      error: (err) => console.error("Error agregando al carrito", err)
    });
  }

}
