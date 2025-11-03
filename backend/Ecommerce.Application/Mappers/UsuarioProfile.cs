using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Login
            CreateMap<UsuarioLoginDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(dto.Correo, dto.Correo, dto.Correo, dto.Contrasena, "Cliente"));

            CreateMap<Usuario, UsuarioLoginDTO>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore());

            // Registro (cliente)
            CreateMap<UsuarioRegistroDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(
                    dto.nombre,
                    dto.apellido,
                    dto.correo,
                    dto.contrasena,
                    dto.tipoCuenta
                ));

            CreateMap<Usuario, UsuarioRegistroDTO>()
                .ForMember(dest => dest.contrasena, opt => opt.Ignore());

            // Registro (admin crea usuario)
            CreateMap<UsuarioRegistroAdminDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(
                    dto.Nombre,
                    dto.Apellido ?? string.Empty,
                    dto.Correo,
                    dto.Contrasena,
                    dto.Rol
                ));

            // Proyección para listar en panel admin
            CreateMap<Usuario, UsuarioAdminDTO>();

            // Actualización (admin modifica usuario)
            // Solo actualiza: Nombre, Apellido, Correo, Rol
            // Ignora claves, contraseña, fechas y relaciones.
            CreateMap<UsuarioActualizarDTO, Usuario>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ContrasenaHash, o => o.Ignore())
                .ForMember(d => d.FechaRegistro, o => o.Ignore())
                .ForMember(d => d.token_recuperacion, o => o.Ignore())
                .ForMember(d => d.fecha_expiracion_token, o => o.Ignore())
                .ForMember(d => d.Direcciones, o => o.Ignore())
                .ForMember(d => d.MetodosPago, o => o.Ignore())
                .ForMember(d => d.Pedidos, o => o.Ignore());
                
            CreateMap<Usuario, UsuarioInfoDTO>();
            CreateMap<Direccion, DireccionDTO>();
            CreateMap<Pedido, PedidoResumenDTO>();
            CreateMap<MetodoPago, MetodoPagoDTO>();

        }
    }
}
