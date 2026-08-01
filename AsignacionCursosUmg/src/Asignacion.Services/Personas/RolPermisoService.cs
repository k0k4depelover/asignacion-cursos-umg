using Asignacion.Data;
using Asignacion.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class RolResumenDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
}

public class PermisoResumenDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
}

public class RolPermisoMatrizDto
{
    public List<RolResumenDto> Roles { get; set; } = new();
    public List<PermisoResumenDto> Permisos { get; set; } = new();
    public HashSet<(int IdRol, int IdPermiso)> Asignados { get; set; } = new();
}

public interface IRolPermisoService
{
    Task<RolPermisoMatrizDto> GetMatrizAsync();
    Task AsignarAsync(int idRol, int idPermiso);
    Task QuitarAsync(int idRol, int idPermiso);
}

public class RolPermisoService(IDbContextFactory<AppDbContext> dbFactory) : IRolPermisoService
{
    public async Task<RolPermisoMatrizDto> GetMatrizAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var roles = await db.Roles.AsNoTracking()
            .OrderBy(r => r.NombreRol)
            .Select(r => new RolResumenDto { Id = r.IdRol, Nombre = r.NombreRol })
            .ToListAsync();

        var permisos = await db.Permisos.AsNoTracking()
            .OrderBy(p => p.NombrePermiso)
            .Select(p => new PermisoResumenDto { Id = p.IdPermiso, Nombre = p.NombrePermiso })
            .ToListAsync();

        var asignados = await db.RolPermisos.AsNoTracking()
            .Select(rp => new { rp.IdRol, rp.IdPermiso })
            .ToListAsync();

        return new RolPermisoMatrizDto
        {
            Roles = roles,
            Permisos = permisos,
            Asignados = asignados.Select(a => (a.IdRol, a.IdPermiso)).ToHashSet()
        };
    }

    public async Task AsignarAsync(int idRol, int idPermiso)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var yaExiste = await db.RolPermisos.AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
        if (yaExiste)
        {
            return;
        }

        db.RolPermisos.Add(new RolPermiso { IdRol = idRol, IdPermiso = idPermiso });
        await db.SaveChangesAsync();
    }

    public async Task QuitarAsync(int idRol, int idPermiso)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.RolPermisos.FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
        if (entidad is null)
        {
            return;
        }

        db.RolPermisos.Remove(entidad);
        await db.SaveChangesAsync();
    }
}
