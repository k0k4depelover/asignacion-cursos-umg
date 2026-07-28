using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class RequisitoCursoService : IRequisitoCursoService
    {
        private readonly AppContext _context;

        public RequisitoCursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequisitoCurso>> ObtenerTodosRequisitosCursosAsync()
        {
            return await _context.RequisitoCursos.ToListAsync();
        }

        public async Task<RequisitoCurso?> ObtenerRequisitoCursoPorIdAsync(int idRequisitoCurso)
        {
            return await _context.RequisitoCursos.FindAsync(idRequisitoCurso);
        }

        public async Task<RequisitoCurso> CrearRequisitoCursoAsync(RequisitoCurso requisitoCurso)
        {
            _context.RequisitoCursos.Add(requisitoCurso);
            await _context.SaveChangesAsync();
            return requisitoCurso;
        }

        public async Task<bool> ActualizarRequisitoCursoAsync(int idRequisitoCurso, RequisitoCurso requisitoCurso)
        {
            var requisitoCursoExistente = await _context.RequisitoCursos.FindAsync(idRequisitoCurso);
            if (requisitoCursoExistente == null)
            {
                return false;
            }

            requisitoCursoExistente.IdRequisitoCurso = requisitoCursoExistente.IdRequisitoCurso;
            requisitoCursoExistente.IdPensumCurso = requisitoCurso.IdPensumCurso;
            requisitoCursoExistente.TipoRequisito = requisitoCurso.TipoRequisito;
            requisitoCursoExistente.IdCursoRequerido = requisitoCurso.IdCursoRequerido;
            requisitoCursoExistente.CreditosMinimos = requisitoCurso.CreditosMinimos;
            requisitoCursoExistente.DescripcionRequisito = requisitoCurso.DescripcionRequisito;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarRequisitoCursoAsync(int idRequisitoCurso)
        {
            var requisitoCursoExistente = await _context.RequisitoCursos.FindAsync(idRequisitoCurso);
            if (requisitoCursoExistente == null)
            {
                return false;
            }
            _context.RequisitoCursos.Remove(requisitoCursoExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
