using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Entities;

using Ecommerce.Application.DTOs;            // CrearDescuentoDto
using Ecommerce.Application.Descuentos.Dtos; // ActualizarDescuentaDto, DescuentoDto

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescuentoController : ControllerBase
    {
        private readonly IDescuentoRepository _repo;
        private readonly IMapper _mapper;

        public DescuentoController(IDescuentoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/descuento/obtenerPorCodigo/SUMMER25 (público)
        [HttpGet("obtenerPorCodigo/{codigo}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorCodigo([FromRoute] string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return BadRequest(new { mensaje = "El código no puede estar vacío." });

                var entity = await _repo.GetByCodigoAsync(codigo.Trim());
                if (entity is null)
                    return NotFound(new { mensaje = $"No se encontró el descuento con código '{codigo}'." });

                var dto = _mapper.Map<DescuentoDto>(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // POST: api/descuento/crearDescuento (solo admin)
        [HttpPost("crearDescuento")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearDescuento([FromBody] CrearDescuentoDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                // Validaciones de caso de uso (sin servicio)
                if (string.IsNullOrWhiteSpace(dto.Codigo))
                    return BadRequest(new { mensaje = "El código no puede estar vacío." });

                if (!ValidarFechasYPorcentaje(dto.FechaInicio, dto.FechaFin, dto.Porcentaje, out var error))
                    return BadRequest(new { mensaje = error });

                var existente = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
                if (existente is not null)
                    return Conflict(new { mensaje = $"Ya existe un descuento con el código '{dto.Codigo}'." });

                // Mapear a dominio (usa tu DescuentoProfile con ConstructUsing)
                var entity = _mapper.Map<Descuento>(dto);

                await _repo.AddAsync(entity);

                var resp = _mapper.Map<DescuentoDto>(entity);

                // Siguiendo tu patrón de ProductoController: 200 OK
                return Ok(resp);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // PUT: api/descuento/actualizarDescuento/{id} (solo admin)
        [HttpPut("actualizarDescuento/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarDescuento([FromRoute] int id, [FromBody] ActualizarDescuentaDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                if (id <= 0)
                    return BadRequest(new { mensaje = "Id inválido." });

                if (dto.Id != 0 && dto.Id != id)
                    return BadRequest(new { mensaje = "El Id de la ruta no coincide con el del cuerpo." });

                dto.Id = id;

                if (string.IsNullOrWhiteSpace(dto.Codigo))
                    return BadRequest(new { mensaje = "El código no puede estar vacío." });

                if (!ValidarFechasYPorcentaje(dto.FechaInicio, dto.FechaFin, dto.Porcentaje, out var error))
                    return BadRequest(new { mensaje = error });

                // Verificar conflicto de código con otro registro
                var conMismoCodigo = await _repo.GetByCodigoAsync(dto.Codigo.Trim());
                if (conMismoCodigo is not null && conMismoCodigo.Id != dto.Id)
                    return Conflict(new { mensaje = $"Ya existe otro descuento con el código '{dto.Codigo}'." });

                var entity = _mapper.Map<Descuento>(dto);

                await _repo.UpdateAsync(entity);

                var resp = _mapper.Map<DescuentoDto>(entity);
                return Ok(resp);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // DELETE: api/descuento/eliminarDescuento/{id} (solo admin)
        [HttpDelete("eliminarDescuento/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarDescuento([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { mensaje = "Id inválido." });

                await _repo.DeleteAsync(id);
                return Ok(new { mensaje = "Descuento eliminado correctamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // --- Helper local (mismas reglas que en el servicio) ---
        private static bool ValidarFechasYPorcentaje(DateTime inicio, DateTime fin, decimal porcentaje, out string? error)
        {
            if (porcentaje <= 0 || porcentaje > 100)
            {
                error = "El porcentaje debe estar entre 0 y 100.";
                return false;
            }
            if (fin <= inicio)
            {
                error = "La fecha de fin debe ser posterior a la fecha de inicio.";
                return false;
            }
            error = null;
            return true;
        }
    }
}
