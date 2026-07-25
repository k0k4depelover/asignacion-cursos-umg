using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IRolService
    {
        Task<List<Rol>> ObtenerTodosRolesAsync();

        Task<Rol?> ObtenerRolPorId(int idRol);

        Task<Rol> CrearRolAsync(Rol rol);

        Task<bool> ActualizarRolAsync(int idRol, Rol rol);

        Task<bool> EliminarRolAsync(int idRol);
    }
}