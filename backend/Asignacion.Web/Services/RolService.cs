using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class RolService : IRolService
    {
        private readonly AppContext _context


            public RolService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ObtenerTodosRolesAsync()
        {
            return await _context.Rol.ToListAsync();
        }

        public async Task<Rol?> ObtenerRolPorIdAsync(int idRol)
        {
            return await _context.Rol.FindAsync(idRol);
        }

        public async Task<Rol> CrearRolAsync(Rol rol)
        {
            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<bool> ActualizarRolAsync(int idRol, Rol rol)
        {
            var rolExistente = await _context.Rol.FindAsync(idRol);
            if (rolExistente == null)
            {
                return false;
            }
            rolExistente.NombreRol = rol.NombreRol;
            rolExistente.DescripcionRol = rol.DescripcionRol;
            rolExistente.EstadoRol = rol.EstadoRol;
            rolExistente.IdRol = rol.IdRol;
            return true;
        }

        public async Task<bool> EliminarRolAsync(int idRol)
        {
            var rolExistente = _context.Rol.FindAsync(idRol);
            if (rolExistente == null)
            {
                return false;
            }
            _context.Rol.Remove(rolExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
