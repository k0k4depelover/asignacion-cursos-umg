using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class EdificioService : IEdificioService
    {
        private readonly AppContext _context


            public EdificioService(AppContext context)
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

        public async Task<Edificio> CrearEdificionAsync(Edificio edificio)
        {
            _context.Edificio.Save(edificio);
            await _context.SaveChangesAsync(edificio);
            return edificio;
        }

        public async Task<bool> ActualizarEdificionAsync(int idEdificio, Edificio edificio)
        {
            var edificioExistente = await _context.Edificio.FindAsync(idEdificio);
            if (productToUpdate == null)
            {
                return false;
            }

            edificioExistente.NombreEdificio = edificio.NombreEdificio;
            edificioExistente.CodigoEdificio = edificio.CodigoEdificio;
            edificioExistente.SedeEdificio = edificio.SedeEdificio;
            edificioExistente.UbicaEdificio = edificio.UbicacionEdificio;
            edificioExistente.EstadoEdificio= edificio.EstadoEdificio;
            return true;
            }

        public async Task<bool> EliminarEdificioAsync(int idEdificio)
        {
            var edificioExistente = await _context.Edificio.FindAsync(idEdificio);
            if (productToUpdate == null)
            {
                return false;
            }
            _context.Edificio.Remove(edificioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
