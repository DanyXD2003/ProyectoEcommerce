using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            // Mapeo: CrearProductoDto -> Producto
            CreateMap<CrearProductoDto, Producto>()
                .ConstructUsing(dto => new Producto(
                    dto.CategoriaId,
                    dto.Nombre,
                    dto.Precio,
                    dto.Stock,
                    dto.Activo,
                    dto.Descripcion
                ));

            // Mapeo: ActualizarProductoDto -> Producto
            CreateMap<ActualizarProductoDto, Producto>()
                .ConstructUsing(dto => new Producto(
                    dto.CategoriaId,
                    dto.Nombre,
                    dto.Precio,
                    dto.Stock,
                    dto.Activo,
                    dto.Descripcion
                ));

            // Mapeo: Producto -> ProductoDto
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre,
                           opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : string.Empty));
        }
    }
}
