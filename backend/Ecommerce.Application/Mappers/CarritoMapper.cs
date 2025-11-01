using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class CarritoProfile : Profile
    {
        public CarritoProfile()
        {
            // arrito -> CarritoDto
            CreateMap<Carrito, CarritoDto>()
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

            //CarritoDetalle -> CarritoDetalleDto
            CreateMap<CarritoDetalle, CarritoDetalleDto>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.Ignore()) // se llenará luego en el servicio
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Cantidad * src.PrecioUnitario));

            //AgregarProductoDto -> CarritoDetalle
            CreateMap<AgregarProductoDto, CarritoDetalle>()
                .ConstructUsing(dto => new CarritoDetalle(
                    0, // Carrito id se asignará luego
                    dto.ProductoId, // Producto id desde el DTO
                    dto.Cantidad, // Cantidad desde el DTO
                    0 // Precio unitario se calculará según el producto real
                ));
        }
    }
}
