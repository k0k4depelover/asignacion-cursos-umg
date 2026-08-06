using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class EdificioService : IEdificioService
    {
        private readonly AppDbContext _context;


            public EdificioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Edificio>> ObtenerTodosEdificiosAsync()
        {
            return await _context.Edificio.ToListAsync();
        }

        public async Task<Edificio?> ObtenerEdificioPorIdAsync(int idEdificio)
        {
            return await _context.Edificio.FindAsync(idEdificio);
        }

        public async Task<Edificio> CrearEdificioAsync(Edificio edificio)
        {
            await _context.Edificio.AddAsync(edificio);
            await _context.SaveChangesAsync();
            return edificio;
        }

        public async Task<bool> ActualizarEdificioAsync(int idEdificio, Edificio edificio)
        {
            var edificioExistente = await _context.Edificio.FindAsync(idEdificio);
            if (edificioExistente == null)
            {
                return false;
            }

            edificioExistente.NombreEdificio = edificio.NombreEdificio;
            edificioExistente.CodigoEdificio = edificio.CodigoEdificio;
            edificioExistente.SedeEdificio = edificio.SedeEdificio;
            edificioExistente.UbicacionEdificio = edificio.UbicacionEdificio;
            edificioExistente.EstadoEdificio= edificio.EstadoEdificio;
            await _context.SaveChangesAsync();
            return true;
            }

        public async Task<bool> EliminarEdificioAsync(int idEdificio)
        {
            var edificioExistente = await _context.Edificio.FindAsync(idEdificio);
            if (edificioExistente == null)
            {
                return false;
            }
            _context.Edificio.Remove(edificioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
