using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class PensumCursoService : IPensumCursoService
    {
        private readonly AppContext _context;


            public PensumCursoService(AppContext context)
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
            _context.PesnumCurso.AddAsync(pensumCurso);
            await _context.SaveChangesAsync();
            return pensumCurso;
        }

        public async Task<bool> ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso)
        {
            var pensumCursoExistente = _context.PensumCurso.FindAsync(idPensumCurso);
            if (pensumCursoExistente == null)
            {
                return false;
            }
            pensumCursoExistente.IdPensumCurso = pensumCurso.IdPensumCurso;
            pensumCursoExistente.IdPensum = pensumCurso.IdPensum;
            pensumCursoExistente.IdCurso = pensumCurso.IdCurso;
            pensumCursoExistente.Ciclo = pensumCurso.Ciclo;
            pensumCursoExistente.EsObligatorio = pensumCurso.EsObligatorio;
            return true;
        }

        public async Task<bool> EliminarPensumCursoAsync(int idPensumCurso)
        {
            var pensumCursoExistente = _context.PensumCurso.FindAsync(idPensumCurso);
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
