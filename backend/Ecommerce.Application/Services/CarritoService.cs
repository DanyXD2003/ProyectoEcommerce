using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
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

        // 🔹 Obtener carrito activo del usuario
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

        // 🔹 Agregar producto al carrito
        public async Task<CarritoDetalleDto> AgregarProductoAsync(int usuarioId, AgregarProductoDto dto)
        {
            // Buscar carrito activo
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null)
            {
                carrito = new Carrito(usuarioId);
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            // Obtener producto
            var producto = await _context.Productos.FindAsync(dto.ProductoId)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

            // Validar stock
            if (dto.Cantidad > producto.Stock)
                throw new ArgumentException("La cantidad solicitada supera el stock disponible.");

            // Verificar si ya existe en el carrito
            var detalleExistente = carrito.Detalles.FirstOrDefault(d => d.ProductoId == dto.ProductoId);

            if (detalleExistente != null)
            {
                int nuevaCantidad = detalleExistente.Cantidad + dto.Cantidad;
                if (nuevaCantidad > producto.Stock)
                    throw new ArgumentException("La cantidad total en el carrito supera el stock disponible.");

                detalleExistente.ActualizarCantidad(nuevaCantidad);
            }
            else
            {
                var nuevoDetalle = new CarritoDetalle(
                    carrito.Id,
                    dto.ProductoId,
                    dto.Cantidad,
                    producto.Precio
                );

                carrito.Detalles.Add(nuevoDetalle);
            }

            await _context.SaveChangesAsync();

            var detalle = carrito.Detalles.First(d => d.ProductoId == dto.ProductoId);
            var detalleDto = _mapper.Map<CarritoDetalleDto>(detalle);
            detalleDto.ProductoNombre = producto.Nombre;

            return detalleDto;
        }

        // 🔹 Nuevo método: Actualizar cantidad de producto
        public async Task<CarritoDetalleDto> ActualizarCantidadProductoAsync(int usuarioId, ActualizarCantidadDto dto)
        {
            if (dto.Cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");

            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo)
                ?? throw new KeyNotFoundException("No hay un carrito activo para este usuario.");

            var detalle = carrito.Detalles.FirstOrDefault(d => d.ProductoId == dto.ProductoId)
                ?? throw new KeyNotFoundException("El producto no está en el carrito.");

            var producto = await _context.Productos.FindAsync(dto.ProductoId)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

            // Validación de stock
            if (dto.Cantidad > producto.Stock)
                throw new ArgumentException("La cantidad solicitada supera el stock disponible.");

            // Si cantidad = 0, eliminar el producto
            if (dto.Cantidad == 0)
            {
                _context.CarritoDetalles.Remove(detalle);
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("El producto fue eliminado del carrito porque la cantidad se estableció en 0.");
            }

            // Actualizar cantidad
            detalle.ActualizarCantidad(dto.Cantidad);
            await _context.SaveChangesAsync();

            var detalleDto = _mapper.Map<CarritoDetalleDto>(detalle);
            detalleDto.ProductoNombre = producto.Nombre;
            return detalleDto;
        }

        // Eliminar un producto del carrito activo
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

        // Vaciar carrito
        public async Task VaciarCarritoAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo)
                ?? throw new KeyNotFoundException("No hay un carrito activo para este usuario.");

            _context.CarritoDetalles.RemoveRange(carrito.Detalles);
            await _context.SaveChangesAsync();
        }

        // Aplicar descuento por código al carrito activo
        public async Task<CarritoDto> AplicarDescuentoAsync(int usuarioId, string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Debe proporcionar un código de descuento válido.");

            // Buscar el carrito activo
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo)
                ?? throw new KeyNotFoundException("No hay un carrito activo para este usuario.");

            if (!carrito.Detalles.Any())
                throw new InvalidOperationException("No se puede aplicar un descuento a un carrito vacío.");

            // Buscar descuento por código (solo activo)
            var descuento = await _context.Descuentos.FirstOrDefaultAsync(d =>
                d.Codigo.ToLower() == codigo.ToLower() && d.Activo);

            if (descuento == null)
                throw new KeyNotFoundException("El código de descuento no existe o no está activo.");

            // Aplicar el descuento
            carrito.AplicarDescuento(descuento);

            // Guardar cambios
            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();

            // Mapear carrito actualizado
            var carritoDto = _mapper.Map<CarritoDto>(carrito);

            // Agregar nombres de productos
            foreach (var detalle in carritoDto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                detalle.ProductoNombre = producto?.Nombre ?? string.Empty;
            }

            return carritoDto;
        }
    }
}
