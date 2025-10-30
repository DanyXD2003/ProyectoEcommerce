using Ecommerce.Domain.Entities;

namespace WebFinal.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Usuario usuario);
    }
}
