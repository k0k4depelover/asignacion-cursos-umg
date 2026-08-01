using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Programacion;

public class PeriodoAcademicoDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public string? Descripcion { get; set; }
    public required string TipoPeriodo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool PermiteInscripcion { get; set; }
    public bool PermiteAsignacion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
}

public interface IPeriodoAcademicoService
{
    Task<List<PeriodoAcademicoDto>> GetAllAsync(bool incluirInactivos = false);
    Task<PeriodoAcademicoDto> CreateAsync(PeriodoAcademicoDto dto);
    Task UpdateAsync(PeriodoAcademicoDto dto);
    Task SetEstadoAsync(int id, bool activo);
    Task<List<PeriodoAcademicoDto>> GetAbiertosParaInscripcionAsync();
}

public class PeriodoAcademicoService(IDbContextFactory<AppDbContext> dbFactory) : IPeriodoAcademicoService
{
    public async Task<List<PeriodoAcademicoDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.PeriodosAcademicos.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(p => p.EstadoPeriodo == EstadoConstantes.Activo);
        }

        return await query
            .OrderByDescending(p => p.FechaInicio)
            .Select(p => new PeriodoAcademicoDto
            {
                Id = p.IdPeriodo,
                Codigo = p.CodigoPeriodo,
                Descripcion = p.DescripcionPeriodo,
                TipoPeriodo = p.TipoPeriodo,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                PermiteInscripcion = p.PermiteInscripcion,
                PermiteAsignacion = p.PermiteAsignacion,
                Estado = p.EstadoPeriodo
            })
            .ToListAsync();
    }

    public async Task<PeriodoAcademicoDto> CreateAsync(PeriodoAcademicoDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new PeriodoAcademico
            {
                CodigoPeriodo = dto.Codigo,
                DescripcionPeriodo = dto.Descripcion,
                TipoPeriodo = dto.TipoPeriodo,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                PermiteInscripcion = dto.PermiteInscripcion,
                PermiteAsignacion = dto.PermiteAsignacion,
                EstadoPeriodo = dto.Estado
            };
            db.PeriodosAcademicos.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdPeriodo;
            return dto;
        }, "Periodo académico");
    }

    public async Task UpdateAsync(PeriodoAcademicoDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.PeriodosAcademicos.FindAsync(dto.Id)
                ?? throw new ServiceException("El periodo académico ya no existe.");
            entidad.CodigoPeriodo = dto.Codigo;
            entidad.DescripcionPeriodo = dto.Descripcion;
            entidad.TipoPeriodo = dto.TipoPeriodo;
            entidad.FechaInicio = dto.FechaInicio;
            entidad.FechaFin = dto.FechaFin;
            entidad.PermiteInscripcion = dto.PermiteInscripcion;
            entidad.PermiteAsignacion = dto.PermiteAsignacion;
            entidad.EstadoPeriodo = dto.Estado;
            await db.SaveChangesAsync();
        }, "Periodo académico");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.PeriodosAcademicos.FindAsync(id)
            ?? throw new ServiceException("El periodo académico ya no existe.");
        entidad.EstadoPeriodo = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }

    public async Task<List<PeriodoAcademicoDto>> GetAbiertosParaInscripcionAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.PeriodosAcademicos
            .AsNoTracking()
            .Where(p => p.PermiteInscripcion && p.EstadoPeriodo == EstadoConstantes.Activo)
            .OrderByDescending(p => p.FechaInicio)
            .Select(p => new PeriodoAcademicoDto
            {
                Id = p.IdPeriodo,
                Codigo = p.CodigoPeriodo,
                Descripcion = p.DescripcionPeriodo,
                TipoPeriodo = p.TipoPeriodo,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                PermiteInscripcion = p.PermiteInscripcion,
                PermiteAsignacion = p.PermiteAsignacion,
                Estado = p.EstadoPeriodo
            })
            .ToListAsync();
    }
}
