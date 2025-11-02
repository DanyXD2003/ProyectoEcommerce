using AutoMapper;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Mappers
{
    public class DescuentoProfile : Profile
    {
        public DescuentoProfile()
        {
            // 🔹 CrearDescuentoDto → Descuento (crear nuevo)
            CreateMap<CrearDescuentoDto, Descuento>()
                .ConstructUsing(dto => new Descuento(
                    dto.Codigo,
                    dto.Descripcion,
                    dto.Porcentaje
                ));

            // 🔹 ActualizarDescuentoDto → Descuento (rehidratación/actualización)
            CreateMap<ActualizarDescuentoDto, Descuento>()
                .ConstructUsing(dto => new Descuento(
                    dto.Id,
                    dto.Codigo,
                    dto.Descripcion,
                    dto.Porcentaje,
                    dto.Activo
                ));

            // 🔹 Descuento → DescuentoDto (para mostrar al frontend)
            CreateMap<Descuento, DescuentoDto>();
        }
    }
}
