using Asignacion.Data;
using Asignacion.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Matricula;

public class InscripcionDto
{
    public int Id { get; set; }
    public DateTime FechaInscripcion { get; set; }
    public decimal CostoInscripcion { get; set; }
    public decimal MontoMensual { get; set; }
    public int CicloInscrito { get; set; }
    public required string EstadoSolvencia { get; set; }
    public required string EstadoInscripcion { get; set; }
    public int IdEstudiante { get; set; }
    public int IdPeriodo { get; set; }
}

public interface IInscripcionService
{
    Task<List<InscripcionDto>> GetByEstudianteAsync(int idEstudiante);
    Task<InscripcionDto?> GetActivaAsync(int idEstudiante, int idPeriodo);
}

public class InscripcionService(IDbContextFactory<AppDbContext> dbFactory) : IInscripcionService
{
    public async Task<List<InscripcionDto>> GetByEstudianteAsync(int idEstudiante)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Inscripciones
            .AsNoTracking()
            .Where(i => i.IdEstudiante == idEstudiante)
            .OrderByDescending(i => i.FechaInscripcion)
            .Select(i => new InscripcionDto
            {
                Id = i.IdInscripcion,
                FechaInscripcion = i.FechaInscripcion,
                CostoInscripcion = i.CostoInscripcion,
                MontoMensual = i.MontoMensual,
                CicloInscrito = i.CicloInscrito,
                EstadoSolvencia = i.EstadoSolvencia,
                EstadoInscripcion = i.EstadoInscripcion,
                IdEstudiante = i.IdEstudiante,
                IdPeriodo = i.IdPeriodo
            })
            .ToListAsync();
    }

    public async Task<InscripcionDto?> GetActivaAsync(int idEstudiante, int idPeriodo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Inscripciones
            .AsNoTracking()
            .Where(i => i.IdEstudiante == idEstudiante && i.IdPeriodo == idPeriodo && i.EstadoInscripcion == EstadoConstantes.Activo)
            .Select(i => new InscripcionDto
            {
                Id = i.IdInscripcion,
                FechaInscripcion = i.FechaInscripcion,
                CostoInscripcion = i.CostoInscripcion,
                MontoMensual = i.MontoMensual,
                CicloInscrito = i.CicloInscrito,
                EstadoSolvencia = i.EstadoSolvencia,
                EstadoInscripcion = i.EstadoInscripcion,
                IdEstudiante = i.IdEstudiante,
                IdPeriodo = i.IdPeriodo
            })
            .FirstOrDefaultAsync();
    }
}
