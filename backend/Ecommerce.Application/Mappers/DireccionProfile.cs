using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class DireccionProfile : Profile
    {
        public DireccionProfile()
        {
            // Crear dirección
            CreateMap<CrearDireccionDto, Direccion>()
                .ConstructUsing(dto => new Direccion(0, dto.Calle, dto.Ciudad, dto.Departamento, dto.CodigoPostal, dto.Pais, dto.Telefono));

            // Actualizar dirección
            CreateMap<ActualizarDireccionDto, Direccion>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Devolver dirección
            CreateMap<Direccion, DireccionDto>();
        }
    }
}
