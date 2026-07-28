using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPermisoService
    {
        Task<List<Permiso>> ObtenerTodosPermisosAsync();

        Task<Permiso?> ObtenerPermisoPorIdAsync(int idPermiso);

        Task<Permiso> CrearPermisoAsync(Permiso permiso);

        Task<bool> ActualizarPermisoAsync(int idPermiso, Permiso permiso);

        Task<bool> EliminarPermisoAsync(int idPermiso);
    }
}