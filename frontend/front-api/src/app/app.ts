import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from "./features/auth/login/login";
import { FormsModule } from '@angular/forms';
import { home } from "./features/home/home";
import { Productos } from "./features/productos/productos";
import { ProfileComponent } from './features/auth/profile/profile';
import { Carrito} from './features/carrito/carrito';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Login, FormsModule, home, Productos, ProfileComponent, Carrito],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('front-api');
}
