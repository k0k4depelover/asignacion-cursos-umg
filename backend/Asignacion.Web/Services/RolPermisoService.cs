using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class RolPermisoService : IRolPermisoService
    {
        private readonly AppDbContext _context;

        public RolPermisoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolPermiso>> ObtenerTodosRolesPermisosAsync()
        {
            return await _context.RolPermiso.ToListAsync();
        }

        public async Task<RolPermiso?> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso)
        {
            return await _context.RolPermiso.FindAsync(idRol, idPermiso);
        }

        public async Task<List<RolPermiso>> ObtenerPorRolAsync(int idRol)
        {
            return await _context.RolPermiso.Where(rp => rp.IdRol == idRol).ToListAsync();
        }

        public async Task<List<RolPermiso>> ObtenerPorPermisoAsync(int idPermiso)
        {
            return await _context.RolPermiso.Where(rp => rp.IdPermiso == idPermiso).ToListAsync();
        }

        public async Task<RolPermiso> CrearRolPermisoAsync(RolPermiso rolPermiso)
        {
            _context.RolPermiso.Add(rolPermiso);
            await _context.SaveChangesAsync();
            return rolPermiso;
        }

        public async Task<bool> EliminarRolPermisoAsync(int idRol, int idPermiso)
        {
            var rolPermisoExistente = await _context.RolPermiso.FindAsync(idRol, idPermiso);
            if (rolPermisoExistente == null)
            {
                return false;
            }

            _context.RolPermiso.Remove(rolPermisoExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
