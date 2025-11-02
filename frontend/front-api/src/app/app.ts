import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from "./features/auth/login/login";
import { FormsModule } from '@angular/forms';
import { home } from "./features/home/home";
import { Productos } from "./features/productos/productos";
import { ProfileComponent } from './features/auth/profile/profile';
import { Carrito} from './features/carrito/carrito';
import { AdminComponent } from './features/administracion/admin/admin';
import { ProductosAdminComponent } from './features/administracion/productos/productos-admin/productos-admin';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Login, FormsModule, home, Productos, ProfileComponent, Carrito, AdminComponent, ProductosAdminComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('front-api');
}
