import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class home implements OnInit {
  isLogged = false;
  userName = '';
  isAdmin: boolean = false;
  
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
  console.log("Token desde home:", localStorage.getItem('access_token'));

  this.isLogged = this.authService.isLoggedIn();
  console.log("¿Está logueado?:", this.isLogged);

  const user = this.authService.getUser();
  console.log("Usuario recuperado:", user);

  this.userName = user ? (user.nombre || user.correo || 'Usuario') : '';
  const Role = this.authService.getUserRole();
  console.log("ROL desde token:", Role);
  this.isAdmin = Role === 'admin';
}


  logout() {
    this.authService.logout();
    this.isLogged = false;
    this.userName = '';
    this.router.navigate(['/home']);
  }
}
