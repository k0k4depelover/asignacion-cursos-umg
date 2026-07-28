using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class FacultadService : IFacultadService
    {
        private readonly AppDbContext _context;

        public FacultadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Facultad>> ObtenerTodasFacultadesAsync()
        {
            return await _context.Facultades.ToListAsync();
        }

        public async Task<Facultad?> ObtenerFacultadPorIdAsync(int idFacultad)
        {
            return await _context.Facultades.FindAsync(idFacultad);
        }

        public async Task<Facultad> CrearFacultadAsync(Facultad facultad)
        {
            _context.Facultades.Add(facultad);
            await _context.SaveChangesAsync();
            return facultad;
        }

        public async Task<bool> ActualizarFacultadAsync(int idFacultad, Facultad facultad)
        {
            var facultadExistente = await _context.Facultades.FindAsync(idFacultad);
            if (facultadExistente == null)
            {
                return false;
            }

            facultadExistente.IdFacultad = facultadExistente.IdFacultad;
            facultadExistente.CodigoFacultad = facultad.CodigoFacultad;
            facultadExistente.NombreFacultad = facultad.NombreFacultad;
            facultadExistente.EstadoFacultad = facultad.EstadoFacultad;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarFacultadAsync(int idFacultad)
        {
            var facultadExistente = await _context.Facultades.FindAsync(idFacultad);
            if (facultadExistente == null)
            {
                return false;
            }
            _context.Facultades.Remove(facultadExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
