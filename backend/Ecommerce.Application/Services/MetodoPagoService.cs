using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Application.Services
{
    public class MetodoPagoService
    {
        private readonly IMetodoPagoRepository _metodoPagoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public MetodoPagoService(IMetodoPagoRepository metodoPagoRepository, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _metodoPagoRepository = metodoPagoRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        //Obtener métodos de pago del usuario autenticado
        public async Task<IEnumerable<MetodoPagoDto>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            var metodos = await _metodoPagoRepository.GetByUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<MetodoPagoDto>>(metodos);
        }

        //Agregar un nuevo método de pago
        public async Task<MetodoPagoDto> AgregarAsync(int usuarioId, CrearMetodoPagoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Tipo))
                throw new ArgumentException("El tipo de método de pago es obligatorio.");

            // Validar fecha de expiración (opcional)
            if (dto.FechaExpiracion.HasValue && dto.FechaExpiracion.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("La fecha de expiración no puede ser anterior a la fecha actual.");

            // Validar usuario
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            var metodo = new MetodoPago(
                usuarioId,
                dto.Tipo,
                dto.NumeroToken,
                dto.Banco,
                dto.FechaExpiracion
            );

            await _metodoPagoRepository.AddAsync(metodo);

            return _mapper.Map<MetodoPagoDto>(metodo);
        }

        // Eliminar método de pago (usuario)
        public async Task EliminarAsync(int usuarioId, int metodoId)
        {
            var metodos = await _metodoPagoRepository.GetByUsuarioAsync(usuarioId);
            var metodo = metodos.FirstOrDefault(m => m.Id == metodoId)
                ?? throw new KeyNotFoundException("Método de pago no encontrado o no pertenece al usuario.");

            await _metodoPagoRepository.DeleteAsync(metodo.Id);
        }

        //Obtener todos los métodos de pago (admin)
        public async Task<IEnumerable<MetodoPagoAdminDto>> ObtenerTodosAdminAsync()
        {
            var metodos = await _metodoPagoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MetodoPagoAdminDto>>(metodos);
        }

        //Actualizar un método de pago (admin)
        public async Task<MetodoPagoAdminDto> ActualizarAsync(ActualizarMetodoPagoDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("El Id es obligatorio.");

            var existente = await _metodoPagoRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Método de pago no encontrado.");

            // Validar usuario destino
            var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId)
                ?? throw new KeyNotFoundException("Usuario no válido.");

            var metodoActualizado = _mapper.Map<MetodoPago>(dto);
            await _metodoPagoRepository.UpdateAsync(metodoActualizado);

            return _mapper.Map<MetodoPagoAdminDto>(metodoActualizado);
        }

        // Eliminar método de pago (admin)
        public async Task EliminarAdminAsync(int id)
        {
            var metodo = await _metodoPagoRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Método de pago no encontrado.");

            await _metodoPagoRepository.DeleteAsync(metodo.Id);
        }
    }
}
