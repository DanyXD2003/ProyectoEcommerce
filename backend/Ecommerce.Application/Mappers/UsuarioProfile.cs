using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Mapea UsuarioLoginDTO -> Usuario (login)
            CreateMap<UsuarioLoginDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(dto.Correo, dto.Correo, dto.Contrasena, "Cliente"));

            // Mapea Usuario -> UsuarioLoginDTO (sin contraseña)
            CreateMap<Usuario, UsuarioLoginDTO>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore());

            //  Mapea UsuarioRegistroDTO -> Usuario (registro)
            CreateMap<UsuarioRegistroDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(
                    dto.Nombre,
                    dto.Correo,
                    dto.Contrasena,
                    dto.TipoCuenta
                ));

            // (Opcional) Si luego querés devolver info al frontend:
            CreateMap<Usuario, UsuarioRegistroDTO>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore());
        }
    }
}
