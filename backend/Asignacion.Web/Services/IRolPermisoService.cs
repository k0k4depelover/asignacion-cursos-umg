using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IRolPermisoService
    {
        Task<List<RolPermiso>> ObtenerTodosRolesPermisosAsync();

        Task<RolPermiso?> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso);

        Task<RolPermiso> CrearRolPermisoAsync(RolPermiso rolPermiso);

        Task<bool> EliminarRolPermisoAsync(int idPermiso);
    }
}
