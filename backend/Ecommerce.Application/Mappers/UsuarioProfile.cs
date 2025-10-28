// Ecommerce.Application/Mappers/UsuarioProfile.cs
using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public class UsuarioProfile:Profile
    {
        public UsuarioProfile()
        {
            // Mapea UsuarioLoginDTO -> Usuario (dominio)
            CreateMap<UsuarioLoginDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(dto.Correo, dto.Correo, dto.Contrasena, "Cliente"));
            // Ajusta parámetros del constructor según tu dominio

            // Si luego querés enviar datos de Usuario -> DTO (por ejemplo, respuesta al frontend)
            CreateMap<Usuario, UsuarioLoginDTO>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore()); // No enviamos la contraseña al frontend
        }
    }

}