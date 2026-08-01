using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Catalogo;

public class CursoDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public int Creditos { get; set; }
    public bool RequiereLaboratorio { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
}

public interface ICursoService
{
    Task<List<CursoDto>> GetAllAsync(bool incluirInactivos = false);
    Task<CursoDto> CreateAsync(CursoDto dto);
    Task UpdateAsync(CursoDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class CursoService(IDbContextFactory<AppDbContext> dbFactory) : ICursoService
{
    public async Task<List<CursoDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Cursos.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(c => c.EstadoCurso == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(c => c.NombreCurso)
            .Select(c => new CursoDto
            {
                Id = c.IdCurso,
                Codigo = c.CodigoCurso,
                Nombre = c.NombreCurso,
                Creditos = c.CreditosCurso,
                RequiereLaboratorio = c.RequiereLaboratorio,
                Estado = c.EstadoCurso
            })
            .ToListAsync();
    }

    public async Task<CursoDto> CreateAsync(CursoDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Curso
            {
                CodigoCurso = dto.Codigo,
                NombreCurso = dto.Nombre,
                CreditosCurso = dto.Creditos,
                RequiereLaboratorio = dto.RequiereLaboratorio,
                EstadoCurso = dto.Estado
            };
            db.Cursos.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdCurso;
            return dto;
        }, "Curso");
    }

    public async Task UpdateAsync(CursoDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Cursos.FindAsync(dto.Id)
                ?? throw new ServiceException("El curso ya no existe.");
            entidad.CodigoCurso = dto.Codigo;
            entidad.NombreCurso = dto.Nombre;
            entidad.CreditosCurso = dto.Creditos;
            entidad.RequiereLaboratorio = dto.RequiereLaboratorio;
            entidad.EstadoCurso = dto.Estado;
            await db.SaveChangesAsync();
        }, "Curso");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Cursos.FindAsync(id)
            ?? throw new ServiceException("El curso ya no existe.");
        entidad.EstadoCurso = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
