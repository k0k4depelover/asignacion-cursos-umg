using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Infraestructura;

public class SalonDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public int Capacidad { get; set; }
    public required string TipoEspacio { get; set; }
    public int Nivel { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdEdificio { get; set; }
}

public interface ISalonService
{
    Task<List<SalonDto>> GetAllAsync(bool incluirInactivos = false);
    Task<SalonDto> CreateAsync(SalonDto dto);
    Task UpdateAsync(SalonDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class SalonService(IDbContextFactory<AppDbContext> dbFactory) : ISalonService
{
    public async Task<List<SalonDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Salones.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(s => s.EstadoSalon == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(s => s.NombreSalon)
            .Select(s => new SalonDto
            {
                Id = s.IdSalon,
                Codigo = s.CodigoSalon,
                Nombre = s.NombreSalon,
                Capacidad = s.CapacidadSalon,
                TipoEspacio = s.TipoEspacio,
                Nivel = s.NivelSalon,
                Estado = s.EstadoSalon,
                IdEdificio = s.IdEdificio
            })
            .ToListAsync();
    }

    public async Task<SalonDto> CreateAsync(SalonDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Salon
            {
                CodigoSalon = dto.Codigo,
                NombreSalon = dto.Nombre,
                CapacidadSalon = dto.Capacidad,
                TipoEspacio = dto.TipoEspacio,
                NivelSalon = dto.Nivel,
                EstadoSalon = dto.Estado,
                IdEdificio = dto.IdEdificio
            };
            db.Salones.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdSalon;
            return dto;
        }, "Salon");
    }

    public async Task UpdateAsync(SalonDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Salones.FindAsync(dto.Id)
                ?? throw new ServiceException("El salon ya no existe.");
            entidad.CodigoSalon = dto.Codigo;
            entidad.NombreSalon = dto.Nombre;
            entidad.CapacidadSalon = dto.Capacidad;
            entidad.TipoEspacio = dto.TipoEspacio;
            entidad.NivelSalon = dto.Nivel;
            entidad.EstadoSalon = dto.Estado;
            entidad.IdEdificio = dto.IdEdificio;
            await db.SaveChangesAsync();
        }, "Salon");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Salones.FindAsync(id)
            ?? throw new ServiceException("El salon ya no existe.");
        entidad.EstadoSalon = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
