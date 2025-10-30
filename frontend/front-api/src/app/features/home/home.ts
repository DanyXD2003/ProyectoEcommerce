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

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.isLogged = this.authService.isLoggedIn();
    const user = this.authService.getUser();
    this.userName = user ? user.nombre : '';
  }

  logout() {
    this.authService.logout();
    this.isLogged = false;
    this.userName = '';
    this.router.navigate(['/home']);
  }
}
