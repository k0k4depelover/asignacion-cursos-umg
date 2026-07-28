using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class PermisoService : IPermisoService
    {
        private readonly AppContext _context;


            public PermisoService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Permiso>> ObtenerTodosPermisosAsync()
        {
            return await _context.Permiso.ToListAsync();
        }

        public async Task<Permiso?> ObtenerPermisoPorIdAsync(int idPermiso)
        {
            return await _context.Permiso.FindAsync(idPermiso);
        }

        public async Task<Permiso> CrearPermisoAsync(Permiso permiso)
        {
            _context.Permiso.AddAsync(permiso);
            await _context.SaveChangesAsync();
            return permiso;
        }

        public async Task<bool> ActualizarPermisoAsync(int idPermiso, Permiso permiso)
        {
            var permisoExistente = _context.Permiso.FindAsync(idPermiso);
            if (permisoExistente == null)
            {
                return false;
            }
            permisoExistente.IdPermiso = permiso.IdPermiso;
            permisoExistente.NombrePermiso = permiso.NombrePermiso;
            permisoExistente.DescripcionPermiso = permiso.DescripcionPermiso;
            return true;
        }

        public async Task<bool> EliminarPermisoAsync(int idPermiso)
        {
            var permisoExistente = _context.Permiso.FindAsync(idPermiso);
            if (permisoExistente == null)
            {
                return false;
            }
            _context.Permiso.Remove(permisoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
