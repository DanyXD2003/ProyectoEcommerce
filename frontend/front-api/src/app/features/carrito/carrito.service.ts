import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AgregarProductoDto {
  productoId: number;
  cantidad: number;
}

export interface CarritoDetalleDto {
  productoId: number;
  productoNombre: string;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
}

/*export interface CarritoDto {
  id: number;
  fechaCreacion: Date;
  activo: boolean;
  detalles: CarritoDetalleDto[];
  total: number;
}*/

export interface CarritoDto {
  id: number;
  fechaCreacion: Date;
  activo: boolean;
  detalles: CarritoDetalleDto[];
  totalSinDescuento: number;
  totalDescuento: number;
  totalConDescuento: number;
}


@Injectable({
  providedIn: 'root'
})
export class CarritoService {

  private baseUrl = 'http://localhost:5000/api/carrito';

  constructor(private http: HttpClient) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('access_token');
    return {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
  }

  obtenerCarrito(): Observable<CarritoDto> {
    return this.http.get<CarritoDto>(`${this.baseUrl}/obtenerActivo`, this.getAuthHeaders());
  }

  agregarProducto(productoId: number, cantidad: number): Observable<any> {
    const body: AgregarProductoDto = { productoId,  cantidad };
    return this.http.post(`${this.baseUrl}/agregarProducto`, body, this.getAuthHeaders());
  }

  eliminarProducto(productoId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/eliminarProducto/${productoId}`, this.getAuthHeaders());
  }

  vaciarCarrito(): Observable<any> {
    return this.http.delete(`${this.baseUrl}/vaciar`, this.getAuthHeaders());
  }

  // DESCUENTOS
  aplicarDescuento(codigo: string): Observable<CarritoDto> {
    const body = { codigo };
    return this.http.post<CarritoDto>(
      `${this.baseUrl}/aplicarDescuento`,
      body,
      this.getAuthHeaders()
    );
  }

  // CREAR PEDIDO
  crearPedido(direccionId: number, metodoPagoId: number, tipoPago: string) {
    const body = { direccionId, metodoPagoId, tipoPago };
    return this.http.post(
      `http://localhost:5000/api/pedido/crear`,
      body,
      this.getAuthHeaders()
    );
  }

}
