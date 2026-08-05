using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Infraestructura;

public class EdificioDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public required string Sede { get; set; }
    public string? Ubicacion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
}

public interface IEdificioService
{
    Task<List<EdificioDto>> GetAllAsync(bool incluirInactivos = false);
    Task<EdificioDto> CreateAsync(EdificioDto dto);
    Task UpdateAsync(EdificioDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class EdificioService(IDbContextFactory<AppDbContext> dbFactory) : IEdificioService
{
    public async Task<List<EdificioDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Edificios.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(e => e.EstadoEdificio == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(e => e.NombreEdificio)
            .Select(e => new EdificioDto
            {
                Id = e.IdEdificio,
                Codigo = e.CodigoEdificio,
                Nombre = e.NombreEdificio,
                Sede = e.Sede,
                Ubicacion = e.Ubicacion,
                Estado = e.EstadoEdificio
            })
            .ToListAsync();
    }

    public async Task<EdificioDto> CreateAsync(EdificioDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Edificio
            {
                CodigoEdificio = dto.Codigo,
                NombreEdificio = dto.Nombre,
                Sede = dto.Sede,
                Ubicacion = dto.Ubicacion,
                EstadoEdificio = dto.Estado
            };
            db.Edificios.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdEdificio;
            return dto;
        }, "Edificio");
    }

    public async Task UpdateAsync(EdificioDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Edificios.FindAsync(dto.Id)
                ?? throw new ServiceException("El edificio ya no existe.");
            entidad.CodigoEdificio = dto.Codigo;
            entidad.NombreEdificio = dto.Nombre;
            entidad.Sede = dto.Sede;
            entidad.Ubicacion = dto.Ubicacion;
            entidad.EstadoEdificio = dto.Estado;
            await db.SaveChangesAsync();
        }, "Edificio");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Edificios.FindAsync(id)
            ?? throw new ServiceException("El edificio ya no existe.");
        entidad.EstadoEdificio = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
