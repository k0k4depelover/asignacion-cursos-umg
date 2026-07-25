using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class FacultadService : IFacultadService
    {
        private readonly AppContext _context


            public FacultadService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Facultad>> ObtenerTodasFacultadesAsync() { 
            return await _context.Facultad.ToListAsync();
        }

        public async Task<Facultad?> ObtenerFacultadPorId(int idFacultad) {
            return await _context.Facultad.FindAsync(idFacultad);
        }

        public async Task<Facultad> CrearFacultadAsync(Facultad facultad) { 
            _context.Facultad.Add(facultad);
            await _context.SaveChangesAsync();
            return facultad;
        }

        public async Task<bool> ActualizarFacultadAsync(int idFacultad, Facultad facultad) { 
            var facultadExistente = await _context.Facultad.FindAsync(facultad);
            if (facultadExistente == null)
            {
                return false;
            }

            facultadExistente.CodigoFacultad = facultad.CodigoFacultad;
            facultadExistente.NombreFacultad = facultad.NombreFacultad;
            facultadExistente.EstadoFacultad = facultad.EstadoFacultad;


        }

        public async Task<bool> EliminarFacultadAsync(int idFacultad) {
            var facultadExistente = await _context.Facultad.FindAsync(facultad);
            if (facultadExistente == null)
            {
                return false;
            }

            _context.Facultad.Remove(facultadExistente);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
