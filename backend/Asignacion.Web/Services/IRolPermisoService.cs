using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPermisoService
    {
        Task<List<Permiso>> ObtenerTodosPermisosAsync();

        Task<RequisitoCurso?> ObtenerPermisoPorIdAsync(int idPermiso);

        Task<RequisitoCurso> CrearPermisoAsync(Permiso permiso);

        Task<bool> ActualizarPermisoAsync(int idPermiso, Permiso permiso);

        Task<bool> EliminarPermisoAsync(int idPermiso);
    }
}