using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class PensumService : IPensumService
    {
        private readonly AppContext _context;

        public PensumService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pensum>> ObtenerTodosPensumsAsync()
        {
            return await _context.Pensums.ToListAsync();
        }

        public async Task<Pensum?> ObtenerPensumPorIdAsync(int idPensum)
        {
            return await _context.Pensums.FindAsync(idPensum);
        }

        public async Task<Pensum> CrearPensumAsync(Pensum pensum)
        {
            _context.Pensums.Add(pensum);
            await _context.SaveChangesAsync();
            return pensum;
        }

        public async Task<bool> ActualizarPensumAsync(int idPensum, Pensum pensum)
        {
            var pensumExistente = await _context.Pensums.FindAsync(idPensum);
            if (pensumExistente == null)
            {
                return false;
            }

            pensumExistente.IdPensum = pensumExistente.IdPensum;
            pensumExistente.CodigoPensum = pensum.CodigoPensum;
            pensumExistente.AnioPensum = pensum.AnioPensum;
            pensumExistente.EstadoPensum = pensum.EstadoPensum;
            pensumExistente.JornadaPensum = pensum.JornadaPensum;
            pensumExistente.IdCarrera = pensum.IdCarrera;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarPensumAsync(int idPensum)
        {
            var pensumExistente = await _context.Pensums.FindAsync(idPensum);
            if (pensumExistente == null)
            {
                return false;
            }
            _context.Pensums.Remove(pensumExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
