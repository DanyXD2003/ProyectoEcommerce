import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { AuthGuard } from './core/guards/auth.guard';
import { home } from './features/home/home';
import { Productos } from './features/productos/productos';
import { recovery } from './features/auth/recovery/recovery';
import { ProfileComponent } from './features/auth/profile/profile';
import { Carrito } from './features/carrito/carrito';
import { AdminComponent } from './features/administracion/admin/admin';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: home },
  { path: 'admin', component: AdminComponent },
  { path: 'productos', component: Productos },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'recovery', component: recovery },
  { path: 'profile', component: ProfileComponent },
  { path: 'carrito', component: Carrito },
  { path: 'dashboard', component: Register, canActivate: [AuthGuard] }
];
