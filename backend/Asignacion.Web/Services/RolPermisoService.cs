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
            return await _context.RolPermiso
                .Include(rp => rp.Rol)
                .Include(rp => rp.Permiso)
                .ToListAsync();
        }

        public async Task<RolPermiso?> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso)
        {
            return await _context.RolPermiso
                .Include(rp => rp.Rol)
                .Include(rp => rp.Permiso)
                .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
        }

        public async Task<List<RolPermiso>> ObtenerPorRolAsync(int idRol)
        {
            return await _context.RolPermiso
                .Include(rp => rp.Permiso)
                .Where(rp => rp.IdRol == idRol)
                .ToListAsync();
        }

        public async Task<List<RolPermiso>> ObtenerPorPermisoAsync(int idPermiso)
        {
            return await _context.RolPermiso
                .Include(rp => rp.Rol)
                .Where(rp => rp.IdPermiso == idPermiso)
                .ToListAsync();
        }

        public async Task<RolPermiso> CrearRolPermisoAsync(RolPermiso rolPermiso)
        {
            _context.RolPermiso.Add(rolPermiso);
            await _context.SaveChangesAsync();
            return rolPermiso;
        }

        public async Task<bool> EliminarRolPermisoAsync(int idRol, int idPermiso)
        {
            var existente = await _context.RolPermiso
                .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

            if (existente == null)
            {
                return false;
            }

            _context.RolPermiso.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}