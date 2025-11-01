using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Application.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;

        public ProductoService(IProductoRepository productoRepository, IMapper mapper, EcommerceDbContext context)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
            _context = context;
        }

        //Crear producto (solo admin)
        public async Task<ProductoDto> CrearProductoAsync(CrearProductoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");
            if (dto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            var producto = _mapper.Map<Producto>(dto);

            await _productoRepository.AddAsync(producto);

            // Incluimos la categoría para devolver su nombre en el DTO
            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            var productoDto = _mapper.Map<ProductoDto>(producto);
            productoDto.CategoriaNombre = categoria?.Nombre ?? string.Empty;

            return productoDto;
        }

        //Actualizar producto (solo admin)
        public async Task<ProductoDto> ActualizarProductoAsync(int id, ActualizarProductoDto dto)
        {
            var productoExistente = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productoExistente == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");
            if (dto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            // Actualizamos propiedades manualmente (sin tocar el Id)
            productoExistente.GetType().GetProperty("Nombre")?.SetValue(productoExistente, dto.Nombre);
            productoExistente.GetType().GetProperty("Descripcion")?.SetValue(productoExistente, dto.Descripcion);
            productoExistente.GetType().GetProperty("Precio")?.SetValue(productoExistente, dto.Precio);
            productoExistente.GetType().GetProperty("Stock")?.SetValue(productoExistente, dto.Stock);
            productoExistente.GetType().GetProperty("Activo")?.SetValue(productoExistente, dto.Activo);
            productoExistente.GetType().GetProperty("CategoriaId")?.SetValue(productoExistente, dto.CategoriaId);

            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            var productoDto = _mapper.Map<ProductoDto>(productoExistente);
            productoDto.CategoriaNombre = categoria?.Nombre ?? string.Empty;

            return productoDto;
        }


        //Eliminar producto (borrado lógico)
        public async Task EliminarProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            producto.GetType().GetProperty("Activo")?.SetValue(producto, false);

            await _context.SaveChangesAsync();
        }

        //Obtener todos los productos activos (público)
        public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDto>>(productos);
        }

        //Obtener todos los productos (admin, incluye inactivos)
        public async Task<IEnumerable<ProductoDto>> ObtenerTodosAdminAsync()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDto>>(productos);
        }

        //Obtener producto por ID (solo activos)
        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            return producto == null ? null : _mapper.Map<ProductoDto>(producto);
        }

        //Obtener productos por categoría (solo activos)
        public async Task<IEnumerable<ProductoDto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId && p.Activo)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDto>>(productos);
        }
    }
}
