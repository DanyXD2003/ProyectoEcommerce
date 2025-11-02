using AutoMapper;
using Ecommerce.Application.Descuentos.Dtos;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Application.Descuentos.Services
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

        public async Task<DescuentoDto?> GetByCodigoAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código no puede estar vacío.", nameof(codigo));

            var entity = await _repo.GetByCodigoAsync(codigo.Trim());
            return entity is null ? null : _mapper.Map<DescuentoDto>(entity);
        }

        public async Task<DescuentoDto> CreateAsync(CrearDescuentoDto dto)
        {
            ValidarFechasYPorcentaje(dto.FechaInicio, dto.FechaFin, dto.Porcentaje);
            if (string.IsNullOrWhiteSpace(dto.Codigo))
                throw new ArgumentException("El código no puede estar vacío.", nameof(dto.Codigo));

            var existente = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
            if (existente is not null)
                throw new InvalidOperationException($"Ya existe un descuento con el código '{dto.Codigo}'.");

            var entity = _mapper.Map<Descuento>(dto);

            await _repo.AddAsync(entity);

            return _mapper.Map<DescuentoDto>(entity);
        }

        public async Task<DescuentoDto> UpdateAsync(ActualizarDescuentaDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("Id inválido.", nameof(dto.Id));

            ValidarFechasYPorcentaje(dto.FechaInicio, dto.FechaFin, dto.Porcentaje);
            if (string.IsNullOrWhiteSpace(dto.Codigo))
                throw new ArgumentException("El código no puede estar vacío.", nameof(dto.Codigo));

            // Evitar duplicados de código en otro registro
            var existenteConMismoCodigo = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
            if (existenteConMismoCodigo is not null && existenteConMismoCodigo.Id != dto.Id)
                throw new InvalidOperationException($"Ya existe otro descuento con el código '{dto.Codigo}'.");

            var entity = _mapper.Map<Descuento>(dto);

            await _repo.UpdateAsync(entity);

            return _mapper.Map<DescuentoDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.", nameof(id));

            await _repo.DeleteAsync(id);
        }

        // --- Helpers ---
        private static void ValidarFechasYPorcentaje(DateTime inicio, DateTime fin, decimal porcentaje)
        {
            if (porcentaje <= 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100.", nameof(porcentaje));

            if (fin <= inicio)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");
        }
    }
}
