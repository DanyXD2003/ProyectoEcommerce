using AutoMapper;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Mappers
{
    public class MetodoPagoProfile : Profile
    {
        public MetodoPagoProfile()
        {
            // De entidad → DTO base
            CreateMap<MetodoPago, MetodoPagoDto>();

            // De entidad → DTO admin (agregamos datos del usuario)
            CreateMap<MetodoPago, MetodoPagoAdminDto>()
                .ForMember(dest => dest.UsuarioCorreo, opt => opt.MapFrom(src => src.Usuario.Correo))
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario.Nombre));

            // De CrearMetodoPagoDto → entidad
            CreateMap<CrearMetodoPagoDto, MetodoPago>()
                .ConstructUsing(dto =>
                    new MetodoPago(
                        0,                      
                        dto.Tipo,
                        dto.NumeroToken,
                        dto.Banco,
                        dto.FechaExpiracion
                    ));

            // De ActualizarMetodoPagoDto → entidad
            CreateMap<ActualizarMetodoPagoDto, MetodoPago>()
                .ConstructUsing(dto =>
                    new MetodoPago(
                        dto.Id,
                        dto.UsuarioId,
                        dto.Tipo,
                        dto.NumeroToken,
                        dto.Banco,
                        dto.FechaExpiracion
                    ));
        }
    }
}
