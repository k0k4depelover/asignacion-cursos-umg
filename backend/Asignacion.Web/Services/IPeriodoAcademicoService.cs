using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPeriodoAcademicoService
    {
        Task<List<PeriodoAcademico>> ObtenerTodosPeriodosAcademicosAsync();

        Task<PeriodoAcademico?> ObtenerPeriodoAcademicoPorId(int idPeriodoAcademico);

        Task<PeriodoAcademico> CrearPeriodoAcademicoAsync(PeriodoAcademico periodoAcademico);

        Task<bool> ActualizarPeriodoAcademicoAsync(int idPeriodoAcademico, PeriodoAcademico periodoAcademico);

        Task<bool> EliminarPeriodoAcademicoAsync(int idPeriodoAcademico);
    }
}