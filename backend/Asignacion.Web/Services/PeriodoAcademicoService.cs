using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class PeriodoAcademicoService : IPeriodoAcademicoService
    {
        private readonly AppContext _context;


            public PeriodoAcademicoService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<PeriodoAcademico>> ObtenerTodosPeriodosAcademicosAsync()
        {
            return await _context.PeriodoAcademico.ToListAsync();
        }

        public async Task<PeriodoAcademico?> ObtenerPeriodoAcademicoPorIdAsync(int idPeriodoAcademico)
        {
            return await _context.PeriodoAcademico.FindAsync(idPeriodoAcademico);
        }

        public async Task<PeriodoAcademico> CrearPeriodoAcademicoAsync(PeriodoAcademico periodoAcademico)
        {
            _context.PeriodoAcademico.AddAsync(periodoAcademico);
            await _context.SaveChangesAsync();
            return periodoAcademico;
        }

        public async Task<bool> ActualizarPeriodoAcademicoAsync(int idPeriodoAcademico, PeriodoAcademico periodoAcademico)
        {
            var periodoAcademicoExistente = _context.Asignacion.FindAsync(idPeriodoAcademico);
            if (asignacionExistente == null)
            {
                return false;
            }
            periodoAcademicoExistente.IdPeriodo = periodoAcademico.IdPeriodo;
            periodoAcademicoExistente.CodigoPeriodo = periodoAcademico.CodigoPeriodo;
            periodoAcademicoExistente.DescripcionPeriodo = periodoAcademico.DescripcionPeriodo;
            periodoAcademicoExistente.TipoPeriodo = periodoAcademico.TipoPeriodo;
            periodoAcademicoExistente.FechaInicio = periodoAcademico.FechaInicio;
            periodoAcademicoExistente.FechaFin = periodoAcademico.FechaFin;
            periodoAcademicoExistente.PermiteInscripcion = periodoAcademico.PermiteInscripcion;
            periodoAcademicoExistente.PermiteAsignacion = periodoAcademico.PermiteAsignacion;
            periodoAcademicoExistente.EstadoPeriodo = periodoAcademico.EstadoPeriodo;
            return true;
        }

        public async Task<bool> EliminarPeriodoAcademicoAsync(int idPeriodoAcademico)
        {
            var periodoAcademicoExistente = _context.PeriodoAcademico.FindAsync(idPeriodoAcademico);
            if (periodoAcademicoExistente == null)
            {
                return false;
            }
            _context.PeriodoAcademico.Remove(periodoAcademicoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
