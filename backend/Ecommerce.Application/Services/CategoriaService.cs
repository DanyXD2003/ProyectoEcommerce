using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, EcommerceDbContext context, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
        {
            var categorias = await _context.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                return null;

            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");

            // Validar duplicados
            var existe = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
                throw new InvalidOperationException($"Ya existe una categoría con el nombre '{dto.Nombre}'.");

            var categoria = _mapper.Map<Categoria>(dto);
            await _categoriaRepository.AddAsync(categoria);

            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<CategoriaDto> ActualizarAsync(ActualizarCategoriaDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("El ID de la categoría es inválido.");

            var existente = await _categoriaRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Categoría no encontrada.");

            // Validar duplicado de nombre en otra categoría
            var duplicado = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower() && c.Id != dto.Id);

            if (duplicado)
                throw new InvalidOperationException($"Ya existe otra categoría con el nombre '{dto.Nombre}'.");

            // Actualizar los campos
            existente = _mapper.Map<Categoria>(dto);
            await _categoriaRepository.UpdateAsync(existente);

            return _mapper.Map<CategoriaDto>(existente);
        }

        public async Task EliminarAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException("Categoría no encontrada.");

            // Validar si tiene productos asociados
            bool tieneProductos = await _context.Productos.AnyAsync(p => p.CategoriaId == id);
            if (tieneProductos)
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados.");

            await _categoriaRepository.DeleteAsync(id);
        }
    }
}
