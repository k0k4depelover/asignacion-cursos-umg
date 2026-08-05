using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class RolDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
}

public interface IRolService
{
    Task<List<RolDto>> GetAllAsync(bool incluirInactivos = false);
    Task<RolDto> CreateAsync(RolDto dto);
    Task UpdateAsync(RolDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class RolService(IDbContextFactory<AppDbContext> dbFactory) : IRolService
{
    private static readonly string[] RolesDelSistema =
    [
        EstadoConstantes.RolAdministrador,
        EstadoConstantes.RolEstudiante,
        EstadoConstantes.RolCatedratico
    ];

    public async Task<List<RolDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Roles.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(r => r.EstadoRol == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(r => r.NombreRol)
            .Select(r => new RolDto { Id = r.IdRol, Nombre = r.NombreRol, Descripcion = r.DescripcionRol, Estado = r.EstadoRol })
            .ToListAsync();
    }

    public async Task<RolDto> CreateAsync(RolDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Rol { NombreRol = dto.Nombre, DescripcionRol = dto.Descripcion, EstadoRol = dto.Estado };
            db.Roles.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdRol;
            return dto;
        }, "Rol");
    }

    public async Task UpdateAsync(RolDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Roles.FindAsync(dto.Id)
                ?? throw new ServiceException("El rol ya no existe.");
            entidad.NombreRol = dto.Nombre;
            entidad.DescripcionRol = dto.Descripcion;
            entidad.EstadoRol = dto.Estado;
            await db.SaveChangesAsync();
        }, "Rol");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Roles.FindAsync(id)
            ?? throw new ServiceException("El rol ya no existe.");

        if (!activo && RolesDelSistema.Contains(entidad.NombreRol))
        {
            throw new ServiceException("No se puede desactivar un rol del sistema.");
        }

        entidad.EstadoRol = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
