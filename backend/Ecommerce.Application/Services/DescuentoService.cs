using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;


namespace Ecommerce.Application.Services
{
    public class DescuentoService
    {
        private readonly IDescuentoRepository _repo;
        private readonly IMapper _mapper;

        public DescuentoService(IDescuentoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // 🔹 Obtener todos los descuentos
        public async Task<IEnumerable<DescuentoDto>> GetAllAsync()
        {
            var descuentos = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<DescuentoDto>>(descuentos);
        }

        // 🔹 Obtener descuento por ID
        public async Task<DescuentoDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.", nameof(id));

            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<DescuentoDto>(entity);
        }

        // 🔹 Obtener descuento por código
        public async Task<DescuentoDto?> GetByCodigoAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código no puede estar vacío.", nameof(codigo));

            var entity = await _repo.GetByCodigoAsync(codigo.Trim());
            return entity is null ? null : _mapper.Map<DescuentoDto>(entity);
        }

        // 🔹 Crear un nuevo descuento
        public async Task<DescuentoDto> CreateAsync(CrearDescuentoDto dto)
        {
            ValidarPorcentaje(dto.Porcentaje);

            if (string.IsNullOrWhiteSpace(dto.Codigo))
                throw new ArgumentException("El código no puede estar vacío.", nameof(dto.Codigo));

            var existente = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
            if (existente is not null)
                throw new InvalidOperationException($"Ya existe un descuento con el código '{dto.Codigo}'.");

            var entity = _mapper.Map<Descuento>(dto);
            await _repo.AddAsync(entity);

            return _mapper.Map<DescuentoDto>(entity);
        }

        // 🔹 Actualizar un descuento existente
        public async Task<DescuentoDto> UpdateAsync(ActualizarDescuentoDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("Id inválido.", nameof(dto.Id));

            ValidarPorcentaje(dto.Porcentaje);

            var existente = await _repo.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Descuento no encontrado.");

            // Evitar duplicados de código
            var existenteCodigo = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
            if (existenteCodigo is not null && existenteCodigo.Id != dto.Id)
                throw new InvalidOperationException($"Ya existe otro descuento con el código '{dto.Codigo}'.");

            var entity = _mapper.Map<Descuento>(dto);
            await _repo.UpdateAsync(entity);

            return _mapper.Map<DescuentoDto>(entity);
        }

        // 🔹 Eliminar un descuento
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.", nameof(id));

            await _repo.DeleteAsync(id);
        }

        // --- Helper ---
        private static void ValidarPorcentaje(decimal porcentaje)
        {
            if (porcentaje <= 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100.");
        }
    }
}
