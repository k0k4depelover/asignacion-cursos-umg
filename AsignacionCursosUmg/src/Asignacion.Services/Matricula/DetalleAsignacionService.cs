using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Matricula;

public class DetalleAsignacionDto
{
    public int Id { get; set; }
    public int IdSeccion { get; set; }
    public string SeccionCodigo { get; set; } = "";
    public string CursoNombre { get; set; } = "";
    public int Creditos { get; set; }
    public string PeriodoCodigo { get; set; } = "";
    public int IdEstudiante { get; set; }
    public string EstudianteCarnet { get; set; } = "";
    public string EstudianteNombre { get; set; } = "";
    public string EstadoDetalle { get; set; } = EstadoConstantes.Activo;
    public decimal CostoLaboratorio { get; set; }
    public decimal? NotaFinal { get; set; }
    public string? Resultado { get; set; }
    public DateTime? FechaResultado { get; set; }
}

public interface IDetalleAsignacionService
{
    Task<List<DetalleAsignacionDto>> GetRosterBySeccionAsync(int idSeccion);
    Task<List<DetalleAsignacionDto>> GetHistorialByEstudianteAsync(int idEstudiante);
    Task GuardarNotaAsync(int idDetalleAsignacion, decimal notaFinal);
}

public class DetalleAsignacionService(IDbContextFactory<AppDbContext> dbFactory) : IDetalleAsignacionService
{
    public async Task<List<DetalleAsignacionDto>> GetRosterBySeccionAsync(int idSeccion)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.DetallesAsignacion.AsNoTracking()
            .Where(d => d.IdSeccion == idSeccion)
            .Select(d => new DetalleAsignacionDto
            {
                Id = d.IdDetalleAsignacion,
                IdSeccion = d.IdSeccion,
                SeccionCodigo = d.Seccion!.CodigoSeccion,
                CursoNombre = d.Seccion!.Curso!.NombreCurso,
                Creditos = d.Seccion!.Curso!.CreditosCurso,
                PeriodoCodigo = d.Seccion!.PeriodoAcademico!.CodigoPeriodo,
                IdEstudiante = d.Asignacion!.Inscripcion!.IdEstudiante,
                EstudianteCarnet = d.Asignacion!.Inscripcion!.Estudiante!.CarnetEstudiante,
                EstudianteNombre = d.Asignacion!.Inscripcion!.Estudiante!.NombresEstudiante + " " + d.Asignacion!.Inscripcion!.Estudiante!.ApellidosEstudiante,
                EstadoDetalle = d.EstadoDetalle,
                CostoLaboratorio = d.CostoLaboratorio,
                NotaFinal = d.NotaFinal,
                Resultado = d.Resultado,
                FechaResultado = d.FechaResultado
            })
            .OrderBy(d => d.EstudianteNombre)
            .ToListAsync();
    }

    public async Task<List<DetalleAsignacionDto>> GetHistorialByEstudianteAsync(int idEstudiante)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.DetallesAsignacion.AsNoTracking()
            .Where(d => d.Asignacion!.Inscripcion!.IdEstudiante == idEstudiante)
            .Select(d => new DetalleAsignacionDto
            {
                Id = d.IdDetalleAsignacion,
                IdSeccion = d.IdSeccion,
                SeccionCodigo = d.Seccion!.CodigoSeccion,
                CursoNombre = d.Seccion!.Curso!.NombreCurso,
                Creditos = d.Seccion!.Curso!.CreditosCurso,
                PeriodoCodigo = d.Seccion!.PeriodoAcademico!.CodigoPeriodo,
                IdEstudiante = idEstudiante,
                EstadoDetalle = d.EstadoDetalle,
                CostoLaboratorio = d.CostoLaboratorio,
                NotaFinal = d.NotaFinal,
                Resultado = d.Resultado,
                FechaResultado = d.FechaResultado
            })
            .OrderByDescending(d => d.PeriodoCodigo)
            .ToListAsync();
    }

    public async Task GuardarNotaAsync(int idDetalleAsignacion, decimal notaFinal)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.DetallesAsignacion.FindAsync(idDetalleAsignacion)
                ?? throw new ServiceException("El registro de asignación ya no existe.");

            entidad.NotaFinal = notaFinal;
            entidad.Resultado = notaFinal >= EstadoConstantes.NotaAprobatoria
                ? EstadoConstantes.ResultadoAprobado
                : EstadoConstantes.ResultadoReprobado;
            entidad.FechaResultado = DateTime.Now;

            await db.SaveChangesAsync();
        }, "Calificación");
    }
}
