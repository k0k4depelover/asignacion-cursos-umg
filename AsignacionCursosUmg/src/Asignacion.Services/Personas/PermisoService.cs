using Asignacion.Data;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class PermisoDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
}

public interface IPermisoService
{
    Task<List<PermisoDto>> GetAllAsync();
    Task<PermisoDto> CreateAsync(PermisoDto dto);
    Task UpdateAsync(PermisoDto dto);
    Task DeleteAsync(int id);
}

public class PermisoService(IDbContextFactory<AppDbContext> dbFactory) : IPermisoService
{
    public async Task<List<PermisoDto>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Permisos
            .AsNoTracking()
            .OrderBy(p => p.NombrePermiso)
            .Select(p => new PermisoDto { Id = p.IdPermiso, Nombre = p.NombrePermiso, Descripcion = p.DescripcionPermiso })
            .ToListAsync();
    }

    public async Task<PermisoDto> CreateAsync(PermisoDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Permiso { NombrePermiso = dto.Nombre, DescripcionPermiso = dto.Descripcion };
            db.Permisos.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdPermiso;
            return dto;
        }, "Permiso");
    }

    public async Task UpdateAsync(PermisoDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Permisos.FindAsync(dto.Id)
                ?? throw new ServiceException("El permiso ya no existe.");
            entidad.NombrePermiso = dto.Nombre;
            entidad.DescripcionPermiso = dto.Descripcion;
            await db.SaveChangesAsync();
        }, "Permiso");
    }

    public async Task DeleteAsync(int id)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Permisos.FindAsync(id)
                ?? throw new ServiceException("El permiso ya no existe.");
            db.Permisos.Remove(entidad);
            await db.SaveChangesAsync();
        }, "Permiso");
    }
}
