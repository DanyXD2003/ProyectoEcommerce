using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class CarritoService
    {
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;

        public CarritoService(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Obtener carrito activo del usuario
        public async Task<CarritoDto?> ObtenerCarritoActivoAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null)
                return null;

            var carritoDto = _mapper.Map<CarritoDto>(carrito);

            // Obtener nombres de productos
            foreach (var detalle in carritoDto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                detalle.ProductoNombre = producto?.Nombre ?? string.Empty;
            }

            return carritoDto;
        }

        //Agregar producto al carrito (crea o actualiza)
        public async Task<CarritoDetalleDto> AgregarProductoAsync(int usuarioId, AgregarProductoDto dto)
        {
            // Buscar carrito activo
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null)
            {
                carrito = new Carrito(usuarioId);
                carrito.GetType().GetProperty("Activo")?.SetValue(carrito, true);
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            // Obtener producto
            var producto = await _context.Productos.FindAsync(dto.ProductoId)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

            // Verificar si ya existe en el carrito
            var detalleExistente = carrito.Detalles.FirstOrDefault(d => d.ProductoId == dto.ProductoId);

            if (detalleExistente != null)
            {
                // Sumar cantidad
                int nuevaCantidad = detalleExistente.Cantidad + dto.Cantidad;

                detalleExistente.GetType().GetProperty("Cantidad")?.SetValue(detalleExistente, nuevaCantidad);
            }
            else
            {
                // Crear nuevo detalle
                var nuevoDetalle = new CarritoDetalle(
                    carrito.Id,
                    dto.ProductoId,
                    dto.Cantidad,
                    producto.Precio
                );

                carrito.Detalles.Add(nuevoDetalle);
            }

            await _context.SaveChangesAsync();

            // Mapear detalle actualizado o creado
            var detalle = carrito.Detalles.First(d => d.ProductoId == dto.ProductoId);
            var detalleDto = _mapper.Map<CarritoDetalleDto>(detalle);
            detalleDto.ProductoNombre = producto.Nombre;

            return detalleDto;
        }

        //Eliminar un producto del carrito activo
        public async Task EliminarProductoAsync(int usuarioId, int productoId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo)
                ?? throw new KeyNotFoundException("No hay un carrito activo para este usuario.");

            var detalle = carrito.Detalles.FirstOrDefault(d => d.ProductoId == productoId)
                ?? throw new KeyNotFoundException("El producto no está en el carrito.");

            _context.CarritoDetalles.Remove(detalle);
            await _context.SaveChangesAsync();
        }

        //Vaciar el carrito (eliminar todos los detalles)
        public async Task VaciarCarritoAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo)
                ?? throw new KeyNotFoundException("No hay un carrito activo para este usuario.");

            _context.CarritoDetalles.RemoveRange(carrito.Detalles);
            await _context.SaveChangesAsync();
        }
    }
}
