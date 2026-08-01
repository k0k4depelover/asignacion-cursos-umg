using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Catalogo;

public class PensumDto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public int Anio { get; set; }
    public required string Jornada { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public int IdCarrera { get; set; }
}

public class PensumCursoDto
{
    public int Id { get; set; }
    public int IdPensum { get; set; }
    public int IdCurso { get; set; }
    public string CursoCodigo { get; set; } = "";
    public string CursoNombre { get; set; } = "";
    public int Ciclo { get; set; }
    public bool EsObligatorio { get; set; } = true;
}

public class RequisitoCursoDto
{
    public int Id { get; set; }
    public int IdPensumCurso { get; set; }
    public required string TipoRequisito { get; set; }
    public int IdCursoRequerido { get; set; }
    public string CursoRequeridoNombre { get; set; } = "";
    public int? CreditosMinimos { get; set; }
    public string? Descripcion { get; set; }
}

public interface IPensumService
{
    Task<List<PensumDto>> GetAllAsync(bool incluirInactivos = false);
    Task<PensumDto> CreateAsync(PensumDto dto);
    Task UpdateAsync(PensumDto dto);
    Task SetEstadoAsync(int id, bool activo);

    Task<List<PensumCursoDto>> GetCursosAsync(int idPensum);
    Task<PensumCursoDto> AgregarCursoAsync(int idPensum, int idCurso, int ciclo, bool esObligatorio);
    Task QuitarCursoAsync(int idPensumCurso);

    Task<List<RequisitoCursoDto>> GetRequisitosAsync(int idPensumCurso);
    Task<RequisitoCursoDto> AgregarRequisitoAsync(RequisitoCursoDto dto);
    Task QuitarRequisitoAsync(int idRequisito);
}

public class PensumService(IDbContextFactory<AppDbContext> dbFactory) : IPensumService
{
    public async Task<List<PensumDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Pensums.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(p => p.EstadoPensum == EstadoConstantes.Activo);
        }

        return await query
            .OrderByDescending(p => p.AnioPensum)
            .Select(p => new PensumDto
            {
                Id = p.IdPensum,
                Codigo = p.CodigoPensum,
                Anio = p.AnioPensum,
                Jornada = p.JornadaPensum,
                Estado = p.EstadoPensum,
                IdCarrera = p.IdCarrera
            })
            .ToListAsync();
    }

    public async Task<PensumDto> CreateAsync(PensumDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Pensum
            {
                CodigoPensum = dto.Codigo,
                AnioPensum = dto.Anio,
                JornadaPensum = dto.Jornada,
                EstadoPensum = dto.Estado,
                IdCarrera = dto.IdCarrera
            };
            db.Pensums.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdPensum;
            return dto;
        }, "Pensum");
    }

    public async Task UpdateAsync(PensumDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Pensums.FindAsync(dto.Id)
                ?? throw new ServiceException("El pensum ya no existe.");
            entidad.CodigoPensum = dto.Codigo;
            entidad.AnioPensum = dto.Anio;
            entidad.JornadaPensum = dto.Jornada;
            entidad.EstadoPensum = dto.Estado;
            entidad.IdCarrera = dto.IdCarrera;
            await db.SaveChangesAsync();
        }, "Pensum");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Pensums.FindAsync(id)
            ?? throw new ServiceException("El pensum ya no existe.");
        entidad.EstadoPensum = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }

    public async Task<List<PensumCursoDto>> GetCursosAsync(int idPensum)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.PensumCursos.AsNoTracking()
            .Where(pc => pc.IdPensum == idPensum)
            .OrderBy(pc => pc.Ciclo)
            .Select(pc => new PensumCursoDto
            {
                Id = pc.IdPensumCurso,
                IdPensum = pc.IdPensum,
                IdCurso = pc.IdCurso,
                CursoCodigo = pc.Curso!.CodigoCurso,
                CursoNombre = pc.Curso!.NombreCurso,
                Ciclo = pc.Ciclo,
                EsObligatorio = pc.EsObligatorio
            })
            .ToListAsync();
    }

    public async Task<PensumCursoDto> AgregarCursoAsync(int idPensum, int idCurso, int ciclo, bool esObligatorio)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();

            var yaExiste = await db.PensumCursos.AnyAsync(pc => pc.IdPensum == idPensum && pc.IdCurso == idCurso);
            if (yaExiste)
            {
                throw new ServiceException("Ese curso ya está asignado a este pensum.");
            }

            var entidad = new PensumCurso { IdPensum = idPensum, IdCurso = idCurso, Ciclo = ciclo, EsObligatorio = esObligatorio };
            db.PensumCursos.Add(entidad);
            await db.SaveChangesAsync();

            var curso = await db.Cursos.AsNoTracking().FirstAsync(c => c.IdCurso == idCurso);
            return new PensumCursoDto
            {
                Id = entidad.IdPensumCurso,
                IdPensum = idPensum,
                IdCurso = idCurso,
                CursoCodigo = curso.CodigoCurso,
                CursoNombre = curso.NombreCurso,
                Ciclo = ciclo,
                EsObligatorio = esObligatorio
            };
        }, "Curso del pensum");
    }

    public async Task QuitarCursoAsync(int idPensumCurso)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.PensumCursos.FindAsync(idPensumCurso);
            if (entidad is null)
            {
                return;
            }

            db.PensumCursos.Remove(entidad);
            await db.SaveChangesAsync();
        }, "Curso del pensum");
    }

    public async Task<List<RequisitoCursoDto>> GetRequisitosAsync(int idPensumCurso)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.RequisitoCursos.AsNoTracking()
            .Where(rc => rc.IdPensumCurso == idPensumCurso)
            .Select(rc => new RequisitoCursoDto
            {
                Id = rc.IdRequisito,
                IdPensumCurso = rc.IdPensumCurso,
                TipoRequisito = rc.TipoRequisito,
                IdCursoRequerido = rc.IdCursoRequerido,
                CursoRequeridoNombre = rc.CursoRequerido!.NombreCurso,
                CreditosMinimos = rc.CreditosMinimos,
                Descripcion = rc.DescripcionRequisito
            })
            .ToListAsync();
    }

    public async Task<RequisitoCursoDto> AgregarRequisitoAsync(RequisitoCursoDto dto)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new RequisitoCurso
            {
                IdPensumCurso = dto.IdPensumCurso,
                TipoRequisito = dto.TipoRequisito,
                IdCursoRequerido = dto.IdCursoRequerido,
                CreditosMinimos = dto.CreditosMinimos,
                DescripcionRequisito = dto.Descripcion
            };
            db.RequisitoCursos.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdRequisito;
            return dto;
        }, "Requisito de curso");
    }

    public async Task QuitarRequisitoAsync(int idRequisito)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.RequisitoCursos.FindAsync(idRequisito);
            if (entidad is null)
            {
                return;
            }

            db.RequisitoCursos.Remove(entidad);
            await db.SaveChangesAsync();
        }, "Requisito de curso");
    }
}
