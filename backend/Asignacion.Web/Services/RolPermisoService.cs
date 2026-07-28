using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class RolPermisoService : IRolPermisoService
    {
        private readonly AppContext _context;

        public RolPermisoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolPermiso>> ObtenerTodosRolesPermisosAsync()
        {
            return await _context.RolPermisos.ToListAsync();
        }

        public async Task<RolPermiso?> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso)
        {
            return await _context.RolPermisos.FindAsync(idRol, idPermiso);
        }

        public async Task<RolPermiso> CrearRolPermisoAsync(RolPermiso rolPermiso)
        {
            _context.RolPermisos.Add(rolPermiso);
            await _context.SaveChangesAsync();
            return rolPermiso;
        }

        public async Task<bool> ActualizarPermisoAsync(int idPermiso, Permiso permiso)
        {
            var permisoExistente = _context.Permiso.FindAsync(idPermiso);
            if (permisoExistente == null)
            {
                return false;
            }
            permisoExistente.IdPermiso = permiso.IdPermiso;
            permisoExistente.IdRol = permiso.IdRol;
            return true;
        }

        public async Task<bool> EliminarPermisoAsync(int idPermiso)
        {
            var permisoExistente = _context.Permiso.FindAsync(idPermiso);
            if (permisoExistente == null)
            {
                return false;
            }
            _context.RolPermisos.Remove(rolPermisoExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
