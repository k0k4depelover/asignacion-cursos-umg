using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Catalogo;

public class FacultadDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
}

public interface IFacultadService
{
    Task<List<FacultadDto>> GetAllAsync(bool incluirInactivos = false);
    Task<FacultadDto> CreateAsync(FacultadDto dto);
    Task UpdateAsync(FacultadDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class FacultadService(IDbContextFactory<AppDbContext> dbFactory) : IFacultadService
{
    public async Task<List<FacultadDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Facultades.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(f => f.EstadoFacultad == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(f => f.NombreFacultad)
            .Select(f => new FacultadDto { Id = f.IdFacultad, Codigo = f.CodigoFacultad, Nombre = f.NombreFacultad, Estado = f.EstadoFacultad })
            .ToListAsync();
    }

    public async Task<FacultadDto> CreateAsync(FacultadDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Facultad { CodigoFacultad = dto.Codigo, NombreFacultad = dto.Nombre, EstadoFacultad = dto.Estado };
            db.Facultades.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdFacultad;
            return dto;
        }, "Facultad");
    }

    public async Task UpdateAsync(FacultadDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Facultades.FindAsync(dto.Id)
                ?? throw new ServiceException("La facultad ya no existe.");
            entidad.CodigoFacultad = dto.Codigo;
            entidad.NombreFacultad = dto.Nombre;
            entidad.EstadoFacultad = dto.Estado;
            await db.SaveChangesAsync();
        }, "Facultad");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Facultades.FindAsync(id)
            ?? throw new ServiceException("La facultad ya no existe.");
        entidad.EstadoFacultad = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
