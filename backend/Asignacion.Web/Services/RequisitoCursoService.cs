using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class RequisitoCursoService : IRequisitoCursoService
    {
        private readonly AppContext _context


            public RequisitoCursoService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<RequisitoCurso>> ObtenerTodosRequisitosCursosAsync()
        {
            return await _context.RequisitoCurso.ToListAsync();
        }

        public async Task<RequisitoCurso?> ObtenerRequisitoCursoPorIdAsync(int idRequisitoCurso)
        {
            return await _context.RequisitoCurso.FindAsync(idRequisitoCurso);
        }

        public async Task<RequisitoCurso> CrearRequisitoCursoAsync(RequisitoCurso requisitoCurso)
        {
            _context.RequisitoCurso.AddAsync(requisitoCurso);
            await _context.SaveChangesAsync();
            return requisitoCurso;
        }

        public async Task<bool> ActualizarRequisitoCursoAsync(int idRequisitoCurso, RequisitoCurso requisitoCurso)
        {
            var requisitoCursoExistente = _context.RequisitoCurso.FindAsync(idRequisitoCurso);
            if (requisitoCursoExistente == null)
            {
                return false;
            }
            requisitoCursoExistente.IdRequisitoCurso = requisitoCurso.IdRequisitoCurso;
            requisitoCursoExistente.IdPensumCurso = requisitoCurso.IdPensumCurso;
            requisitoCursoExistente.TipoRequisito = requisitoCurso.TipoRequisito;
            requisitoCursoExistente.IdCursoRequerido = requisitoCurso.IdCursoRequerido;
            requisitoCursoExistente.CreditosMinimos = requisitoCurso.CreditosMinimos;
            requisitoCursoExistente.DescripcionRequisito = requisitoCurso.DescripcionRequisito;
            return true
        }

        public async Task<bool> EliminarRequisitoCursoAsync(int idRequisitoCurso)
        {
            var requisitoCursoExistente = _context.RequisitoCurso.FindAsync(idRequisitoCurso);
            if (requisitoCursoExistente == null)
            {
                return false;
            }
            _context.RequisitoCurso.Remove(requisitoCursoExistente);
            await _context.SaveChangesAsync();
            return true
        }

    }
}
