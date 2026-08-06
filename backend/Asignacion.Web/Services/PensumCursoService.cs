using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class PensumCursoService : IPensumCursoService
    {
        private readonly AppDbContext _context;


            public PensumCursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PensumCurso>> ObtenerTodosPensumsCursosAsync()
        {
            return await _context.PensumCurso.ToListAsync();
        }

        public async Task<PensumCurso?> ObtenerPensumCursoPorIdAsync(int idPensumCurso)
        {
            return await _context.PensumCurso.FindAsync(idPensumCurso);
        }

        public async Task<PensumCurso> CrearPensumCursoAsync(PensumCurso pensumCurso)
        {
            await _context.PensumCurso.AddAsync(pensumCurso);
            await _context.SaveChangesAsync();
            return pensumCurso;
        }

        public async Task<bool> ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso)
        {
            var pensumCursoExistente = await _context.PensumCurso.FindAsync(idPensumCurso);
            if (pensumCursoExistente == null)
            {
                return false;
            }
            pensumCursoExistente.IdPensumCurso = pensumCurso.IdPensumCurso;
            pensumCursoExistente.IdPensum = pensumCurso.IdPensum;
            pensumCursoExistente.IdCurso = pensumCurso.IdCurso;
            pensumCursoExistente.Ciclo = pensumCurso.Ciclo;
            pensumCursoExistente.EsObligatorio = pensumCurso.EsObligatorio;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarPensumCursoAsync(int idPensumCurso)
        {
            var pensumCursoExistente = await _context.PensumCurso.FindAsync(idPensumCurso);
            if (pensumCursoExistente == null)
            {
                return false;
            }
            _context.PensumCurso.Remove(pensumCursoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
