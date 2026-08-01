using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Programacion;

public class SeccionDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Jornada { get; set; }
    public int CupoMaximo { get; set; }
    public int CupoOcupado { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;

    public int IdCurso { get; set; }
    public string CursoNombre { get; set; } = "";
    public bool CursoRequiereLaboratorio { get; set; }

    public int IdPeriodo { get; set; }
    public string PeriodoCodigo { get; set; } = "";

    public int IdCatedratico { get; set; }
    public string CatedraticoNombre { get; set; } = "";

    public int IdSalon { get; set; }
    public string SalonNombre { get; set; } = "";

    public int CupoDisponible => CupoMaximo - CupoOcupado;
}

public class HorarioSeccionDto
{
    public int Id { get; set; }
    public int IdSeccion { get; set; }
    public required string DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public required string TipoSesion { get; set; }
}

public class SeccionLaboratorioDto
{
    public int Id { get; set; }
    public int IdSeccion { get; set; }
    public int IdLaboratorio { get; set; }
    public string LaboratorioNombre { get; set; } = "";
    public required string DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public decimal CostoExtra { get; set; }
}

public interface ISeccionService
{
    Task<List<SeccionDto>> GetAllAsync(bool incluirInactivos = false, int? idPeriodo = null);
    Task<SeccionDto> CreateAsync(SeccionDto dto);
    Task UpdateAsync(SeccionDto dto);
    Task SetEstadoAsync(int id, bool activo);

    Task<List<HorarioSeccionDto>> GetHorariosAsync(int idSeccion);
    Task<HorarioSeccionDto> AgregarHorarioAsync(HorarioSeccionDto dto);
    Task QuitarHorarioAsync(int idHorario);

    Task<List<SeccionLaboratorioDto>> GetLaboratoriosAsync(int idSeccion);
    Task<SeccionLaboratorioDto> AgregarLaboratorioAsync(SeccionLaboratorioDto dto);
    Task QuitarLaboratorioAsync(int idSeccionLaboratorio);

    Task<List<SeccionDto>> GetByCatedraticoAsync(int idCatedratico, int? idPeriodo = null);
    Task<List<SeccionDto>> GetAbiertasParaPeriodoAsync(int idPeriodo);
}

public class SeccionService(IDbContextFactory<AppDbContext> dbFactory) : ISeccionService
{
    private static IQueryable<SeccionDto> ProjectDto(IQueryable<Seccion> query) => query
        .Select(s => new SeccionDto
        {
            Id = s.IdSeccion,
            Codigo = s.CodigoSeccion,
            Jornada = s.Jornada,
            CupoMaximo = s.CupoMaximo,
            CupoOcupado = s.DetallesAsignacion.Count(d => d.EstadoDetalle == EstadoConstantes.Activo),
            Estado = s.EstadoSeccion,
            IdCurso = s.IdCurso,
            CursoNombre = s.Curso!.NombreCurso,
            CursoRequiereLaboratorio = s.Curso!.RequiereLaboratorio,
            IdPeriodo = s.IdPeriodo,
            PeriodoCodigo = s.PeriodoAcademico!.CodigoPeriodo,
            IdCatedratico = s.IdCatedratico,
            CatedraticoNombre = s.Catedratico!.NombresCatedratico + " " + s.Catedratico!.ApellidosCatedratico,
            IdSalon = s.IdSalon,
            SalonNombre = s.Salon!.NombreSalon
        });

    public async Task<List<SeccionDto>> GetAllAsync(bool incluirInactivos = false, int? idPeriodo = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Secciones.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(s => s.EstadoSeccion == EstadoConstantes.Activo);
        }

        if (idPeriodo is not null)
        {
            query = query.Where(s => s.IdPeriodo == idPeriodo);
        }

        return await ProjectDto(query).OrderBy(s => s.Codigo).ToListAsync();
    }

    public async Task<SeccionDto> CreateAsync(SeccionDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Seccion
            {
                CodigoSeccion = dto.Codigo,
                Jornada = dto.Jornada,
                CupoMaximo = dto.CupoMaximo,
                EstadoSeccion = dto.Estado,
                IdCurso = dto.IdCurso,
                IdPeriodo = dto.IdPeriodo,
                IdCatedratico = dto.IdCatedratico,
                IdSalon = dto.IdSalon
            };
            db.Secciones.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdSeccion;
            return dto;
        }, "Sección");
    }

    public async Task UpdateAsync(SeccionDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Secciones.FindAsync(dto.Id)
                ?? throw new ServiceException("La sección ya no existe.");
            entidad.CodigoSeccion = dto.Codigo;
            entidad.Jornada = dto.Jornada;
            entidad.CupoMaximo = dto.CupoMaximo;
            entidad.EstadoSeccion = dto.Estado;
            entidad.IdCurso = dto.IdCurso;
            entidad.IdPeriodo = dto.IdPeriodo;
            entidad.IdCatedratico = dto.IdCatedratico;
            entidad.IdSalon = dto.IdSalon;
            await db.SaveChangesAsync();
        }, "Sección");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Secciones.FindAsync(id)
            ?? throw new ServiceException("La sección ya no existe.");
        entidad.EstadoSeccion = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }

    public async Task<List<HorarioSeccionDto>> GetHorariosAsync(int idSeccion)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.HorariosSeccion.AsNoTracking()
            .Where(h => h.IdSeccion == idSeccion)
            .Select(h => new HorarioSeccionDto
            {
                Id = h.IdHorario,
                IdSeccion = h.IdSeccion,
                DiaSemana = h.DiaSemana,
                HoraInicio = h.HoraInicio,
                HoraFin = h.HoraFin,
                TipoSesion = h.TipoSesion
            })
            .ToListAsync();
    }

    public async Task<HorarioSeccionDto> AgregarHorarioAsync(HorarioSeccionDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new HorarioSeccion
            {
                IdSeccion = dto.IdSeccion,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                TipoSesion = dto.TipoSesion
            };
            db.HorariosSeccion.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdHorario;
            return dto;
        }, "Horario de sección");
    }

    public async Task QuitarHorarioAsync(int idHorario)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.HorariosSeccion.FindAsync(idHorario);
            if (entidad is null)
            {
                return;
            }

            db.HorariosSeccion.Remove(entidad);
            await db.SaveChangesAsync();
        }, "Horario de sección");
    }

    public async Task<List<SeccionLaboratorioDto>> GetLaboratoriosAsync(int idSeccion)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SeccionesLaboratorio.AsNoTracking()
            .Where(sl => sl.IdSeccion == idSeccion)
            .Select(sl => new SeccionLaboratorioDto
            {
                Id = sl.IdSeccionLaboratorio,
                IdSeccion = sl.IdSeccion,
                IdLaboratorio = sl.IdLaboratorio,
                LaboratorioNombre = sl.Laboratorio!.NombreLaboratorio,
                DiaSemana = sl.DiaSemana,
                HoraInicio = sl.HoraInicio,
                HoraFin = sl.HoraFin,
                CostoExtra = sl.CostoExtra
            })
            .ToListAsync();
    }

    public async Task<SeccionLaboratorioDto> AgregarLaboratorioAsync(SeccionLaboratorioDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new SeccionLaboratorio
            {
                IdSeccion = dto.IdSeccion,
                IdLaboratorio = dto.IdLaboratorio,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                CostoExtra = dto.CostoExtra
            };
            db.SeccionesLaboratorio.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdSeccionLaboratorio;
            return dto;
        }, "Laboratorio de sección");
    }

    public async Task QuitarLaboratorioAsync(int idSeccionLaboratorio)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.SeccionesLaboratorio.FindAsync(idSeccionLaboratorio);
            if (entidad is null)
            {
                return;
            }

            db.SeccionesLaboratorio.Remove(entidad);
            await db.SaveChangesAsync();
        }, "Laboratorio de sección");
    }

    public async Task<List<SeccionDto>> GetByCatedraticoAsync(int idCatedratico, int? idPeriodo = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Secciones.AsNoTracking().Where(s => s.IdCatedratico == idCatedratico);
        if (idPeriodo is not null)
        {
            query = query.Where(s => s.IdPeriodo == idPeriodo);
        }

        return await ProjectDto(query).OrderBy(s => s.Codigo).ToListAsync();
    }

    public async Task<List<SeccionDto>> GetAbiertasParaPeriodoAsync(int idPeriodo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Secciones.AsNoTracking()
            .Where(s => s.IdPeriodo == idPeriodo && s.EstadoSeccion == EstadoConstantes.Activo);

        return await ProjectDto(query).OrderBy(s => s.CursoNombre).ToListAsync();
    }
}
