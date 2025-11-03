using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            // Mapea Pedido → PedidoDto (resumen)
            CreateMap<Pedido, PedidoDto>()
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src =>
                    $"{src.Direccion.Calle}, {src.Direccion.Ciudad}, {src.Direccion.Departamento}"))
                .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src =>
                    src.MetodoPago != null
                        ? $"{src.MetodoPago.Tipo} {src.MetodoPago.Banco} (••••{src.MetodoPago.NumeroToken})"
                        : "ContraEntrega"));

            // Mapea Pedido → PedidoAdminDto
            CreateMap<Pedido, PedidoAdminDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.UsuarioCorreo, opt => opt.MapFrom(src => src.Usuario.Correo))
                .ForMember(dest => dest.DireccionEnvio, opt => opt.MapFrom(src =>
                    $"{src.Direccion.Calle}, {src.Direccion.Ciudad}, {src.Direccion.Departamento}"))
                .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src =>
                    src.MetodoPago != null
                        ? $"{src.MetodoPago.Tipo} {src.MetodoPago.Banco} (••••{src.MetodoPago.NumeroToken})"
                        : "ContraEntrega"));

            // Mapea PedidoDetalle → PedidoDetalleDto
            CreateMap<PedidoDetalle, PedidoDetalleDto>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.PrecioUnitario * src.Cantidad));

            // Mapea Pedido → PedidoDetalleCompletoDto (pedido completo)
            CreateMap<Pedido, PedidoDetalleCompletoDto>()
                .ForMember(dest => dest.DireccionEnvio, opt => opt.MapFrom(src =>
                    $"{src.Direccion.Calle}, {src.Direccion.Ciudad}, {src.Direccion.Departamento}"))
                .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src =>
                    src.MetodoPago != null
                        ? $"{src.MetodoPago.Tipo} {src.MetodoPago.Banco} (••••{src.MetodoPago.NumeroToken})"
                        : "ContraEntrega"))
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));
        }
    }
}
