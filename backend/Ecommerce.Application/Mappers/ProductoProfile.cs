using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            // Mapear Producto (dominio) -> InfoProductoDTO
            CreateMap<Producto, InfoProductoDTO>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio));
        }
    }
}
