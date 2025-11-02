using AutoMapper;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Descuentos.Dtos;
using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Descuentos.Mapping
{
    public class DescuentoProfile : Profile
    {
        public DescuentoProfile()
        {
            // CrearDescuentoDto → Descuento
            CreateMap<CrearDescuentoDto, Descuento>()
                .ConstructUsing(dto => new Descuento(
                    dto.Codigo,
                    dto.Porcentaje,
                    dto.FechaInicio,
                    dto.FechaFin,
                    dto.Descripcion,
                    dto.Activo
                ));

            // ActualizarDescuentaDto → Descuento
            CreateMap<ActualizarDescuentaDto, Descuento>()
                .ConstructUsing(dto => new Descuento(
                    dto.Id,
                    dto.Codigo,
                    dto.Descripcion,
                    dto.Porcentaje,
                    dto.FechaInicio,
                    dto.FechaFin,
                    dto.Activo
                ));

            // Descuento → DescuentoDto
            CreateMap<Descuento, DescuentoDto>();
        }
    }
}
