import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';
import { CategoriaService } from '../../categoria/categoria.service';

@Component({
  selector: 'app-categoria-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterModule],
  templateUrl: './categoria-admin.html',
  styleUrls: ['./categoria-admin.css']
})
export class CategoriaAdminComponent implements OnInit {

  categorias: any[] = [];

  apiUrl = 'https://proyectoecommerceback.onrender.com/api/Categoria';

  categoriaForm = {
    id: 0,
    nombre: '',
    descripcion: ''
  };

  esEdicion = false;

  constructor(private http: HttpClient, private categoriaService: CategoriaService) {}

  ngOnInit() {
    this.cargarCategorias();
  }

  cargarCategorias() {
    this.categoriaService.obtenerTodas()
      .subscribe((res: any) => {
        console.log("Categorías:", res);
        this.categorias = res.map((c: any) => ({
          ...c,
          idCategoria: c.id // mapeo interno igual que productos
        }));
      });
  }

  abrirCrear() {
    this.esEdicion = false;
    this.categoriaForm = {
      id: 0,
      nombre: '',
      descripcion: ''
    };
    (document.getElementById("modalCategoria") as any).style.display = "block";
  }

  abrirEditar(c: any) {
    this.esEdicion = true;
    this.categoriaForm = { ...c };
    (document.getElementById("modalCategoria") as any).style.display = "block";
  }

  guardar() {
    if (!this.categoriaForm.nombre.trim()) {
      alert("El nombre de la categoría es obligatorio");
      return;
    }

    if (this.esEdicion) {
      this.http.put(`${this.apiUrl}/actualizar`, this.categoriaForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarCategorias();
        }, (err) => {
          alert(err.error?.mensaje || "Error al actualizar categoría");
        });
    } else {
      this.http.post(`${this.apiUrl}/crear`, this.categoriaForm)
        .subscribe(() => {
          this.cerrarModal();
          this.cargarCategorias();
        }, (err) => {
          alert(err.error?.mensaje || "Error al crear categoría");
        });
    }
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar categoría?')) return;

    this.http.delete(`${this.apiUrl}/eliminar/${id}`)
      .subscribe(() => {
        this.cargarCategorias();
      }, (err) => {
        alert(err.error?.mensaje || "Error al eliminar categoría");
      });
  }

  cerrarModal() {
    (document.getElementById("modalCategoria") as any).style.display = "none";
  }
}
