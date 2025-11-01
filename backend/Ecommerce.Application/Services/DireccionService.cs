using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using AutoMapper;

namespace Ecommerce.Application.Services
{
    public class DireccionService
    {
        private readonly IDireccionRepository _direccionRepository;
        private readonly IMapper _mapper;

        public DireccionService(IDireccionRepository direccionRepository, IMapper mapper)
        {
            _direccionRepository = direccionRepository;
            _mapper = mapper;
        }

        public async Task<DireccionDto> CrearDireccionAsync(int usuarioId, CrearDireccionDto dto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Calle)) throw new ArgumentException("La calle no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(dto.Ciudad)) throw new ArgumentException("La ciudad no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(dto.Pais)) throw new ArgumentException("El país no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(dto.Telefono)) throw new ArgumentException("El teléfono no puede estar vacío.");

            var direccion = _mapper.Map<Direccion>(dto);
            direccion = new Direccion(usuarioId, dto.Calle, dto.Ciudad, dto.Departamento, dto.CodigoPostal, dto.Pais, dto.Telefono);

            await _direccionRepository.AddAsync(direccion);
            return _mapper.Map<DireccionDto>(direccion);
        }

        public async Task<DireccionDto> ActualizarDireccionAsync(int usuarioId, int direccionId, ActualizarDireccionDto dto)
        {
            var lista = await _direccionRepository.ListByUsuarioAsync(usuarioId);
            var direccion = lista.FirstOrDefault(d => d.Id == direccionId);
            if (direccion == null) throw new KeyNotFoundException("Dirección no encontrada o no pertenece al usuario.");

            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Calle)) throw new ArgumentException("La calle no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(dto.Ciudad)) throw new ArgumentException("La ciudad no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(dto.Pais)) throw new ArgumentException("El país no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(dto.Telefono)) throw new ArgumentException("El teléfono no puede estar vacío.");

            direccion = new Direccion(direccion.Id, usuarioId, dto.Calle, dto.Ciudad, dto.Departamento, dto.CodigoPostal, dto.Pais, dto.Telefono);
            await _direccionRepository.UpdateAsync(direccion);

            return _mapper.Map<DireccionDto>(direccion);
        }

        public async Task EliminarDireccionAsync(int usuarioId, int direccionId)
        {
            var lista = await _direccionRepository.ListByUsuarioAsync(usuarioId);
            var direccion = lista.FirstOrDefault(d => d.Id == direccionId);
            if (direccion == null) throw new KeyNotFoundException("Dirección no encontrada o no pertenece al usuario.");

            // Verificar pedidos asociados
            if (direccion.Pedidos != null && direccion.Pedidos.Any())
                throw new InvalidOperationException("No se puede eliminar la dirección porque hay pedidos asociados.");

            await _direccionRepository.DeleteAsync(direccionId);
        }
    }
}
