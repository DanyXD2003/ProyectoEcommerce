using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Usuario usuario);
    }

