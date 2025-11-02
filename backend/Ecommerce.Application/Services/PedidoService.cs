using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class PedidoService
    {
        private readonly EcommerceDbContext _context;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoDetalleRepository _detalleRepository;
        private readonly IMapper _mapper;

        public PedidoService(
            EcommerceDbContext context,
            IPedidoRepository pedidoRepository,
            IPedidoDetalleRepository detalleRepository,
            IMapper mapper)
        {
            _context = context;
            _pedidoRepository = pedidoRepository;
            _detalleRepository = detalleRepository;
            _mapper = mapper;
        }


        // Crear un pedido a partir del carrito activo del usuario.
        public async Task<PedidoDetalleCompletoDto> CrearPedidoAsync(int usuarioId, CrearPedidoDto dto)
        {
            // Obtener el carrito activo del usuario
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                    .ThenInclude(cd => cd.Carrito)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Activo);

            if (carrito == null || !carrito.Detalles.Any())
                throw new InvalidOperationException("El carrito está vacío o no existe.");

            // Calcular total
            var total = carrito.TotalConDescuento;

            // Validar dirección
            var direccion = await _context.Direcciones.FindAsync(dto.DireccionId)
                ?? throw new KeyNotFoundException("La dirección seleccionada no existe.");

            // Validar método de pago (si aplica)
            MetodoPago? metodoPago = null;
            if (dto.TipoPago == "Tarjeta")
            {
                metodoPago = await _context.MetodosPago.FindAsync(dto.MetodoPagoId)
                    ?? throw new KeyNotFoundException("El método de pago seleccionado no existe.");
            }

            // Crear entidad Pedido
            var pedido = new Pedido(
                usuarioId,
                direccion.Id,
                carrito.Id,
                total,
                dto.TipoPago,
                metodoPago?.Id
            );

            // Crear detalles del pedido
            var detalles = carrito.Detalles.Select(cd =>
                new PedidoDetalle(
                    pedido.Id,
                    cd.ProductoId,
                    cd.Cantidad,
                    cd.PrecioUnitario
                )).ToList();

            pedido.AsociarDetalles(detalles);

            // Guardar en BD
            await _pedidoRepository.AddAsync(pedido);
            await _detalleRepository.AddRangeAsync(detalles);

            // Desactivar carrito
            carrito.Desactivar();
            await _context.SaveChangesAsync();

            // Retornar pedido completo
            var pedidoCompleto = await _pedidoRepository.GetByIdAsync(pedido.Id)
                ?? throw new Exception("No se pudo recuperar el pedido recién creado.");

            return _mapper.Map<PedidoDetalleCompletoDto>(pedidoCompleto);
        }


        // Obtener todos los pedidos del usuario autenticado.
        public async Task<IEnumerable<PedidoDto>> ObtenerPedidosUsuarioAsync(int usuarioId)
        {
            var pedidos = await _pedidoRepository.GetByUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
        }

        // Obtener un pedido específico del usuario.
        public async Task<PedidoDetalleCompletoDto?> ObtenerPedidoPorIdAsync(int usuarioId, int pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null || pedido.UsuarioId != usuarioId)
                throw new KeyNotFoundException("Pedido no encontrado o no pertenece al usuario.");

            return _mapper.Map<PedidoDetalleCompletoDto>(pedido);
        }


        // Listar todos los pedidos (con información del usuario, dirección, pago).
        public async Task<IEnumerable<PedidoAdminDto>> ObtenerTodosAsync()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PedidoAdminDto>>(pedidos);
        }

        // Obtener un pedido específico (vista admin).
        public async Task<PedidoDetalleCompletoDto?> ObtenerPorIdAdminAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            return _mapper.Map<PedidoDetalleCompletoDto>(pedido);
        }

        // Actualizar el estado de un pedido.
        public async Task<PedidoAdminDto> ActualizarEstadoAsync(ActualizarEstadoPedidoDto dto)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(dto.PedidoId)
                ?? throw new KeyNotFoundException("Pedido no encontrado.");

            pedido.ActualizarEstado(dto.NuevoEstado);
            await _pedidoRepository.UpdateAsync(pedido);

            return _mapper.Map<PedidoAdminDto>(pedido);
        }

        // Eliminar un pedido (solo admin o mantenimiento).
        public async Task EliminarAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado.");

            await _pedidoRepository.DeleteAsync(id);
        }
    }
}
