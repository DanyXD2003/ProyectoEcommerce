using System.Diagnostics;
using System.Xml;
using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Entities;

namespace commerce.Application.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile (){

            CreateMap< CrearPedidoDTO, Pedido>()
            .ConstructUsing  (dto => new Pedido(dto.UsuarioId,  dto.DireccionId ,dto.MetodoPagoId ));
      
        }
    }
}