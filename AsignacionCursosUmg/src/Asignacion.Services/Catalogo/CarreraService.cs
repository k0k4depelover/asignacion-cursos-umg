using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Catalogo;

public class CarreraDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public int TotalCiclos { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdFacultad { get; set; }
}

public interface ICarreraService
{
    Task<List<CarreraDto>> GetAllAsync(bool incluirInactivos = false);
    Task<CarreraDto> CreateAsync(CarreraDto dto);
    Task UpdateAsync(CarreraDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class CarreraService(IDbContextFactory<AppDbContext> dbFactory) : ICarreraService
{
    public async Task<List<CarreraDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Carreras.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(c => c.EstadoCarrera == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(c => c.NombreCarrera)
            .Select(c => new CarreraDto
            {
                Id = c.IdCarrera,
                Codigo = c.CodigoCarrera,
                Nombre = c.NombreCarrera,
                TotalCiclos = c.TotalCiclos,
                Estado = c.EstadoCarrera,
                IdFacultad = c.IdFacultad
            })
            .ToListAsync();
    }

    public async Task<CarreraDto> CreateAsync(CarreraDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Carrera
            {
                CodigoCarrera = dto.Codigo,
                NombreCarrera = dto.Nombre,
                TotalCiclos = dto.TotalCiclos,
                EstadoCarrera = dto.Estado,
                IdFacultad = dto.IdFacultad
            };
            db.Carreras.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdCarrera;
            return dto;
        }, "Carrera");
    }

    public async Task UpdateAsync(CarreraDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Carreras.FindAsync(dto.Id)
                ?? throw new ServiceException("La carrera ya no existe.");
            entidad.CodigoCarrera = dto.Codigo;
            entidad.NombreCarrera = dto.Nombre;
            entidad.TotalCiclos = dto.TotalCiclos;
            entidad.EstadoCarrera = dto.Estado;
            entidad.IdFacultad = dto.IdFacultad;
            await db.SaveChangesAsync();
        }, "Carrera");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Carreras.FindAsync(id)
            ?? throw new ServiceException("La carrera ya no existe.");
        entidad.EstadoCarrera = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
