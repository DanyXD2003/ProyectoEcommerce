import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterLink, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-productos-admin',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterModule, FormsModule],
  templateUrl: './productos-admin.html',
  styleUrls: ['./productos-admin.css']
})
export class ProductosAdminComponent implements OnInit {

  productos: any[] = [];
  apiUrl = 'http://localhost:5000/api/Producto';

  // Campos para modal
  productoForm = {
    idProducto: 0,
    nombre: '',
    descripcion: '',
    precio: 0,
    activo: true
  };

  esEdicion = false;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.cargarProductos();
  }

  cargarProductos() {
    this.http.get(`${this.apiUrl}/obtenerTodosAdmin`).subscribe((res: any) => {
      this.productos = res;
    });
  }

  abrirCrear() {
    this.esEdicion = false;
    this.productoForm = {
      idProducto: 0,
      nombre: '',
      descripcion: '',
      precio: 0,
      activo: true
    };
    (document.getElementById("modalProducto") as any).style.display = "block";
  }

  abrirEditar(p: any) {
    this.esEdicion = true;
    this.productoForm = { ...p };
    (document.getElementById("modalProducto") as any).style.display = "block";
  }

  guardar() {
    if (!this.productoForm.nombre || this.productoForm.precio <= 0) {
      alert("Nombre y precio obligatorios");
      return;
    }

    if (this.esEdicion) {
      this.http.put(`${this.apiUrl}/actualizarProducto/${this.productoForm.idProducto}`, this.productoForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarProductos();
        });

    } else {
      this.http.post(`${this.apiUrl}/crearProducto`, this.productoForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarProductos();
        });
    }
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar producto?')) return;

    this.http.delete(`${this.apiUrl}/eliminarProducto/${id}`).subscribe(() => {
      this.cargarProductos();
    });
  }

  cerrarModal() {
    (document.getElementById("modalProducto") as any).style.display = "none";
  }
}
