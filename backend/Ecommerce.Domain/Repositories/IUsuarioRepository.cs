// Ecommerce.Domain/Repositories/IUsuarioRepository.cs
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    // Interfaz que define todas las operaciones que podemos hacer con Usuario
    public interface IUsuarioRepository
    {
        // Obtener un usuario por su Id
        //Task<Usuario?> GetByIdAsync(int id);

        // Obtener un usuario por correo (para login)
        Task<Usuario?> GetByCorreoAsync(string correo);

        // Agregar un nuevo usuario
        Task AddAsync(Usuario usuario);

        // Actualizar datos de un usuario existente
        //Task UpdateAsync(Usuario usuario);

        // Eliminar un usuario por Id
        //Task DeleteAsync(int id);
    }
}
