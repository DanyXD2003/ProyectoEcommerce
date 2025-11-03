import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-usuario-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterModule],
  templateUrl: './usuario-admin.html',
  styleUrls: ['./usuario-admin.css']
})
export class UsuarioAdminComponent implements OnInit {

  usuarios: any[] = [];
  apiUrl = 'https://proyectoecommerceback.onrender.com/api/usuario';

  usuarioForm = {
    id: 0,
    nombre: '',
    apellido: '',
    correo: '',
    contrasena: '',
    rol: 'cliente'
  };

  esEdicion = false;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.cargarUsuarios();
  }

  cargarUsuarios() {
    this.http.get(`${this.apiUrl}/TraerTodos`).subscribe((res: any) => {
      console.log("Usuarios:", res);
      this.usuarios = res.map((u: any) => ({
        ...u,
        idUsuario: u.id
      }));
    });
  }

  abrirCrear() {
    this.esEdicion = false;
    this.usuarioForm = {
      id: 0,
      nombre: '',
      apellido: '',
      correo: '',
      contrasena: '',
      rol: 'cliente'
    };
    (document.getElementById("modalUsuario") as any).style.display = "block";
  }

  abrirEditar(u: any) {
    this.esEdicion = true;
    this.usuarioForm = { ...u, contrasena: '' }; // Contraseña no se edita
    (document.getElementById("modalUsuario") as any).style.display = "block";
  }

  guardar() {
    if (!this.usuarioForm.nombre.trim() || !this.usuarioForm.correo.trim() || !this.usuarioForm.rol.trim()) {
      alert("Nombre, correo y rol son obligatorios");
      return;
    }

    if (this.esEdicion) {
      this.http.put(`${this.apiUrl}/ModificarUsuario/${this.usuarioForm.id}`, this.usuarioForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarUsuarios();
        }, (err) => {
          alert(err.error?.mensaje || "Error al actualizar usuario");
        });
    } else {
      if (!this.usuarioForm.contrasena.trim()) {
        alert("La contraseña es obligatoria al crear un usuario");
        return;
      }
      this.http.post(`${this.apiUrl}/AgregarUsuario`, this.usuarioForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarUsuarios();
        }, (err) => {
          alert(err.error?.mensaje || "Error al crear usuario");
        });
    }
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar usuario?')) return;

    this.http.delete(`${this.apiUrl}/EliminarUsuario/${id}`)
      .subscribe(() => {
        this.cargarUsuarios();
      }, (err) => {
        alert(err.error?.mensaje || "Error al eliminar usuario");
      });
  }

  cerrarModal() {
    (document.getElementById("modalUsuario") as any).style.display = "none";
  }
}
