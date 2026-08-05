using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Infraestructura;

public class LaboratorioDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdSalon { get; set; }
}

public interface ILaboratorioService
{
    Task<List<LaboratorioDto>> GetAllAsync(bool incluirInactivos = false);
    Task<LaboratorioDto> CreateAsync(LaboratorioDto dto);
    Task UpdateAsync(LaboratorioDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class LaboratorioService(IDbContextFactory<AppDbContext> dbFactory) : ILaboratorioService
{
    public async Task<List<LaboratorioDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Laboratorios.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(l => l.EstadoLaboratorio == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(l => l.NombreLaboratorio)
            .Select(l => new LaboratorioDto
            {
                Id = l.IdLaboratorio,
                Nombre = l.NombreLaboratorio,
                Descripcion = l.DescripcionLaboratorio,
                Estado = l.EstadoLaboratorio,
                IdSalon = l.IdSalon
            })
            .ToListAsync();
    }

    public async Task<LaboratorioDto> CreateAsync(LaboratorioDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Laboratorio
            {
                NombreLaboratorio = dto.Nombre,
                DescripcionLaboratorio = dto.Descripcion,
                EstadoLaboratorio = dto.Estado,
                IdSalon = dto.IdSalon
            };
            db.Laboratorios.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdLaboratorio;
            return dto;
        }, "Laboratorio");
    }

    public async Task UpdateAsync(LaboratorioDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Laboratorios.FindAsync(dto.Id)
                ?? throw new ServiceException("El laboratorio ya no existe.");
            entidad.NombreLaboratorio = dto.Nombre;
            entidad.DescripcionLaboratorio = dto.Descripcion;
            entidad.EstadoLaboratorio = dto.Estado;
            entidad.IdSalon = dto.IdSalon;
            await db.SaveChangesAsync();
        }, "Laboratorio");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Laboratorios.FindAsync(id)
            ?? throw new ServiceException("El laboratorio ya no existe.");
        entidad.EstadoLaboratorio = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
