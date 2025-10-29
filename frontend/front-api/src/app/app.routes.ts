import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { AuthGuard } from './core/guards/auth.guard';
import { home } from './features/home/home';
import { productos } from './features/productos/productos';
import { recovery } from './features/auth/recovery/recovery';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: home },
  { path: 'productos', component: productos },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'recovery', component: recovery },
  { path: 'dashboard', component: Register, canActivate: [AuthGuard] }
];
