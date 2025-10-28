import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from "./features/auth/login/login";
import { FormsModule } from '@angular/forms';
import { home } from "./features/home/home";
import { productos } from "./features/productos/productos";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Login, FormsModule, home, productos],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('front-api');
}
