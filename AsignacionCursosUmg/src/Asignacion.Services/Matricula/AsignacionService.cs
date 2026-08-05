using Asignacion.Data;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Matricula;

public class AsignacionDto
{
    public int Id { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public decimal SubtotalLaboratorios { get; set; }
    public decimal TotalPago { get; set; }
    public required string EstadoAsignacion { get; set; }
    public int IdInscripcion { get; set; }
}

public interface IAsignacionService
{
    Task<AsignacionDto?> GetByInscripcionAsync(int idInscripcion);
}

public class AsignacionService(IDbContextFactory<AppDbContext> dbFactory) : IAsignacionService
{
    public async Task<AsignacionDto?> GetByInscripcionAsync(int idInscripcion)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Asignaciones
            .AsNoTracking()
            .Where(a => a.IdInscripcion == idInscripcion)
            .Select(a => new AsignacionDto
            {
                Id = a.IdAsignacion,
                FechaAsignacion = a.FechaAsignacion,
                SubtotalLaboratorios = a.SubtotalLaboratorios,
                TotalPago = a.TotalPago,
                EstadoAsignacion = a.EstadoAsignacion,
                IdInscripcion = a.IdInscripcion
            })
            .FirstOrDefaultAsync();
    }
}
