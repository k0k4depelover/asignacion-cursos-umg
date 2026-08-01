using Asignacion.Data;
using Asignacion.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Reportes;

public class AdminDashboardDto
{
    public int EstudiantesActivos { get; set; }
    public int SeccionesAbiertasPeriodoActual { get; set; }
    public double OcupacionPromedioPorcentaje { get; set; }
    public decimal IngresoProyectadoPeriodoActual { get; set; }
    public string? PeriodoActualCodigo { get; set; }
}

public interface IReporteService
{
    Task<AdminDashboardDto> GetDashboardAsync();
}

public class ReporteService(IDbContextFactory<AppDbContext> dbFactory) : IReporteService
{
    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var estudiantesActivos = await db.Estudiantes.AsNoTracking()
            .CountAsync(e => e.EstadoEstudiante == EstadoConstantes.Activo);

        var periodoActual = await db.PeriodosAcademicos.AsNoTracking()
            .Where(p => p.EstadoPeriodo == EstadoConstantes.Activo)
            .OrderByDescending(p => p.FechaInicio)
            .FirstOrDefaultAsync();

        if (periodoActual is null)
        {
            return new AdminDashboardDto
            {
                EstudiantesActivos = estudiantesActivos,
                SeccionesAbiertasPeriodoActual = 0,
                OcupacionPromedioPorcentaje = 0,
                IngresoProyectadoPeriodoActual = 0,
                PeriodoActualCodigo = null
            };
        }

        var secciones = await db.Secciones.AsNoTracking()
            .Where(s => s.IdPeriodo == periodoActual.IdPeriodo && s.EstadoSeccion == EstadoConstantes.Activo)
            .Select(s => new
            {
                s.CupoMaximo,
                Ocupados = s.DetallesAsignacion.Count(d => d.EstadoDetalle == EstadoConstantes.Activo)
            })
            .ToListAsync();

        var ocupacionPromedio = secciones.Count == 0
            ? 0
            : secciones.Average(s => s.CupoMaximo == 0 ? 0 : (double)s.Ocupados / s.CupoMaximo * 100);

        var ingresoProyectado = await db.Asignaciones.AsNoTracking()
            .Where(a => a.Inscripcion!.IdPeriodo == periodoActual.IdPeriodo && a.EstadoAsignacion == EstadoConstantes.Activo)
            .SumAsync(a => a.TotalPago);

        return new AdminDashboardDto
        {
            EstudiantesActivos = estudiantesActivos,
            SeccionesAbiertasPeriodoActual = secciones.Count,
            OcupacionPromedioPorcentaje = Math.Round(ocupacionPromedio, 1),
            IngresoProyectadoPeriodoActual = ingresoProyectado,
            PeriodoActualCodigo = periodoActual.CodigoPeriodo
        };
    }
}
