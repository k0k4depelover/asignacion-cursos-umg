using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosUsuariosAsync();

        Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario);

        Task<Usuario> CrearUsuarioAsync(Usuario usuario);

        Task<bool> ActualizarUsuarioAsync(int idUsuario, Usuario usuario);

        Task<bool> EliminarUsuarioAsync(int idUsuario);
    }
}