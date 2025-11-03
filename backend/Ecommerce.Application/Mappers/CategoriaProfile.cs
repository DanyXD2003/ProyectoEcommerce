using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            // CrearCategoriaDto → Categoria
            CreateMap<CrearCategoriaDto, Categoria>()
                .ConstructUsing(dto => new Categoria(dto.Nombre, dto.Descripcion));

            // ActualizarCategoriaDto → Categoria (rehidratación)
            CreateMap<ActualizarCategoriaDto, Categoria>()
                .ConstructUsing(dto => new Categoria(dto.Id, dto.Nombre, dto.Descripcion));

            // Categoria → CategoriaDto (para respuesta al frontend)
            CreateMap<Categoria, CategoriaDto>();
        }
    }
}
