using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Usuario;

namespace Asignacion.Web.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosUsuariosAsync();

        Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario);

        Task<Usuario> CrearUsuarioAsync(CreateUserDto dto);

        Task<bool> ActualizarUsuarioAsync(int idUsuario, UpdateUserDto dto);

        Task<bool> EliminarUsuarioAsync(int idUsuario);

    }
}