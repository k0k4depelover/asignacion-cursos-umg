using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class CatedraticoDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Dpi { get; set; }
    public required string Nombres { get; set; }
    public required string Apellidos { get; set; }
    public string? Telefono { get; set; }
    public string? Profesion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdUsuario { get; set; }
    public string? UsuarioCorreo { get; set; }

    public string NombreCompleto => $"{Nombres} {Apellidos}";
}

public interface ICatedraticoService
{
    Task<List<CatedraticoDto>> GetAllAsync(bool incluirInactivos = false);
    Task<CatedraticoDto> CreateAsync(CatedraticoDto dto);
    Task UpdateAsync(CatedraticoDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class CatedraticoService(IDbContextFactory<AppDbContext> dbFactory) : ICatedraticoService
{
    public async Task<List<CatedraticoDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Catedraticos.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(c => c.EstadoCatedratico == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(c => c.ApellidosCatedratico).ThenBy(c => c.NombresCatedratico)
            .Select(c => new CatedraticoDto
            {
                Id = c.IdCatedratico,
                Codigo = c.CodigoCatedratico,
                Dpi = c.DpiCatedratico,
                Nombres = c.NombresCatedratico,
                Apellidos = c.ApellidosCatedratico,
                Telefono = c.TelefonoCatedratico,
                Profesion = c.ProfesionCatedratico,
                Estado = c.EstadoCatedratico,
                IdUsuario = c.IdUsuario,
                UsuarioCorreo = c.Usuario!.CorreoLogin
            })
            .ToListAsync();
    }

    public async Task<CatedraticoDto> CreateAsync(CatedraticoDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Catedratico
            {
                CodigoCatedratico = dto.Codigo,
                DpiCatedratico = dto.Dpi,
                NombresCatedratico = dto.Nombres,
                ApellidosCatedratico = dto.Apellidos,
                TelefonoCatedratico = dto.Telefono,
                ProfesionCatedratico = dto.Profesion,
                EstadoCatedratico = dto.Estado,
                IdUsuario = dto.IdUsuario
            };
            db.Catedraticos.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdCatedratico;
            return dto;
        }, "Catedrático");
    }

    public async Task UpdateAsync(CatedraticoDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Catedraticos.FindAsync(dto.Id)
                ?? throw new ServiceException("El catedrático ya no existe.");
            entidad.CodigoCatedratico = dto.Codigo;
            entidad.DpiCatedratico = dto.Dpi;
            entidad.NombresCatedratico = dto.Nombres;
            entidad.ApellidosCatedratico = dto.Apellidos;
            entidad.TelefonoCatedratico = dto.Telefono;
            entidad.ProfesionCatedratico = dto.Profesion;
            entidad.EstadoCatedratico = dto.Estado;
            await db.SaveChangesAsync();
        }, "Catedrático");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Catedraticos.FindAsync(id)
            ?? throw new ServiceException("El catedrático ya no existe.");
        entidad.EstadoCatedratico = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
