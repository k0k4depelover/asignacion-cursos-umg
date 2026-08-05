using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class EstudianteDto
{
    public int Id { get; set; }
    public required string Carnet { get; set; }
    public required string Dpi { get; set; }
    public required string Nombres { get; set; }
    public required string Apellidos { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public int CicloActual { get; set; } = 1;
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdUsuario { get; set; }
    public string? UsuarioCorreo { get; set; }
    public int IdPensum { get; set; }
    public string PensumCodigo { get; set; } = "";
}

public interface IEstudianteService
{
    Task<List<EstudianteDto>> GetAllAsync(bool incluirInactivos = false);
    Task<EstudianteDto?> GetByIdAsync(int id);
    Task<EstudianteDto> CreateAsync(EstudianteDto dto);
    Task UpdateAsync(EstudianteDto dto);
    Task SetEstadoAsync(int id, bool activo);
}

public class EstudianteService(IDbContextFactory<AppDbContext> dbFactory) : IEstudianteService
{
    private static IQueryable<EstudianteDto> ProjectDto(IQueryable<Estudiante> query) => query
        .Select(e => new EstudianteDto
        {
            Id = e.IdEstudiante,
            Carnet = e.CarnetEstudiante,
            Dpi = e.DpiEstudiante,
            Nombres = e.NombresEstudiante,
            Apellidos = e.ApellidosEstudiante,
            FechaNacimiento = e.FechaNacimiento,
            Direccion = e.DireccionEstudiante,
            Telefono = e.TelefonoEstudiante,
            CicloActual = e.CicloActual,
            Estado = e.EstadoEstudiante,
            IdUsuario = e.IdUsuario,
            UsuarioCorreo = e.Usuario!.CorreoLogin,
            IdPensum = e.IdPensum,
            PensumCodigo = e.Pensum!.CodigoPensum
        });

    public async Task<List<EstudianteDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Estudiantes.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(e => e.EstadoEstudiante == EstadoConstantes.Activo);
        }

        return await ProjectDto(query).OrderBy(e => e.Apellidos).ThenBy(e => e.Nombres).ToListAsync();
    }

    public async Task<EstudianteDto?> GetByIdAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await ProjectDto(db.Estudiantes.AsNoTracking().Where(e => e.IdEstudiante == id)).FirstOrDefaultAsync();
    }

    public async Task<EstudianteDto> CreateAsync(EstudianteDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Estudiante
            {
                CarnetEstudiante = dto.Carnet,
                DpiEstudiante = dto.Dpi,
                NombresEstudiante = dto.Nombres,
                ApellidosEstudiante = dto.Apellidos,
                FechaNacimiento = dto.FechaNacimiento,
                DireccionEstudiante = dto.Direccion,
                TelefonoEstudiante = dto.Telefono,
                CicloActual = dto.CicloActual,
                EstadoEstudiante = dto.Estado,
                IdUsuario = dto.IdUsuario,
                IdPensum = dto.IdPensum
            };
            db.Estudiantes.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdEstudiante;
            return dto;
        }, "Estudiante");
    }

    public async Task UpdateAsync(EstudianteDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Estudiantes.FindAsync(dto.Id)
                ?? throw new ServiceException("El estudiante ya no existe.");
            entidad.CarnetEstudiante = dto.Carnet;
            entidad.DpiEstudiante = dto.Dpi;
            entidad.NombresEstudiante = dto.Nombres;
            entidad.ApellidosEstudiante = dto.Apellidos;
            entidad.FechaNacimiento = dto.FechaNacimiento;
            entidad.DireccionEstudiante = dto.Direccion;
            entidad.TelefonoEstudiante = dto.Telefono;
            entidad.CicloActual = dto.CicloActual;
            entidad.EstadoEstudiante = dto.Estado;
            entidad.IdPensum = dto.IdPensum;
            await db.SaveChangesAsync();
        }, "Estudiante");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Estudiantes.FindAsync(id)
            ?? throw new ServiceException("El estudiante ya no existe.");
        entidad.EstadoEstudiante = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }
}
