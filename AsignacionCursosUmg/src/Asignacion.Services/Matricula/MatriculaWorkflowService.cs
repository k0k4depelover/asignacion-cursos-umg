using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Matricula;

public class CursoElegibleDto
{
    public int IdCurso { get; set; }
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public int Creditos { get; set; }
    public bool RequiereLaboratorio { get; set; }
    public int Ciclo { get; set; }
    public bool CumpleRequisitos { get; set; }
    public string? MotivoNoElegible { get; set; }
}

public class HorarioResumenDto
{
    public required string DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}

public class SeccionInscribibleDto
{
    public int IdSeccion { get; set; }
    public string Codigo { get; set; } = "";
    public string Jornada { get; set; } = "";
    public string CatedraticoNombre { get; set; } = "";
    public string SalonNombre { get; set; } = "";
    public int CupoMaximo { get; set; }
    public int CupoOcupado { get; set; }
    public int CupoDisponible => CupoMaximo - CupoOcupado;
    public List<HorarioResumenDto> Horarios { get; set; } = new();
    public List<HorarioResumenDto> HorariosLaboratorio { get; set; } = new();
    public decimal CostoLaboratorio { get; set; }
}

public class ResultadoInscripcionDto
{
    public int IdInscripcion { get; set; }
    public int IdAsignacion { get; set; }
    public decimal CostoInscripcion { get; set; }
    public decimal MontoMensual { get; set; }
    public decimal SubtotalLaboratorios { get; set; }
    public decimal TotalPago { get; set; }
    public List<string> SeccionesInscritas { get; set; } = new();
}

public interface IMatriculaWorkflowService
{
    Task<List<CursoElegibleDto>> GetCursosElegiblesAsync(int idEstudiante, int idPeriodo);
    Task<List<SeccionInscribibleDto>> GetSeccionesDisponiblesAsync(int idPeriodo, int idCurso);
    Task<ResultadoInscripcionDto> InscribirAsync(int idEstudiante, int idPeriodo, List<int> idsSecciones);
    Task<List<SeccionInscribibleDto>> GetMiHorarioAsync(int idEstudiante, int idPeriodo);
}

public class MatriculaWorkflowService(IDbContextFactory<AppDbContext> dbFactory) : IMatriculaWorkflowService
{
    public async Task<List<CursoElegibleDto>> GetCursosElegiblesAsync(int idEstudiante, int idPeriodo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var estudiante = await db.Estudiantes.AsNoTracking().FirstOrDefaultAsync(e => e.IdEstudiante == idEstudiante)
            ?? throw new ServiceException("El estudiante no existe.");

        var cursosAprobadosIds = await CursosAprobadosAsync(db, idEstudiante);
        var creditosAprobados = await db.DetallesAsignacion.AsNoTracking()
            .Where(d => d.Resultado == EstadoConstantes.ResultadoAprobado
                        && d.Asignacion!.Inscripcion!.IdEstudiante == idEstudiante)
            .Select(d => d.Seccion!.Curso!.CreditosCurso)
            .SumAsync();

        var cursosDelPensum = await db.PensumCursos.AsNoTracking()
            .Where(pc => pc.IdPensum == estudiante.IdPensum && pc.Ciclo <= estudiante.CicloActual)
            .Select(pc => new { pc.IdPensumCurso, pc.Ciclo, Curso = pc.Curso! })
            .ToListAsync();

        var resultado = new List<CursoElegibleDto>();
        foreach (var pc in cursosDelPensum)
        {
            if (cursosAprobadosIds.Contains(pc.Curso.IdCurso))
            {
                continue;
            }

            var requisitos = await db.RequisitoCursos.AsNoTracking()
                .Where(rc => rc.IdPensumCurso == pc.IdPensumCurso)
                .ToListAsync();

            string? motivo = null;
            foreach (var req in requisitos)
            {
                if (req.TipoRequisito == EstadoConstantes.TipoRequisitoCursoAprobado)
                {
                    if (!cursosAprobadosIds.Contains(req.IdCursoRequerido))
                    {
                        var cursoReq = await db.Cursos.AsNoTracking().FirstAsync(c => c.IdCurso == req.IdCursoRequerido);
                        motivo = $"Requiere haber aprobado {cursoReq.NombreCurso}";
                        break;
                    }
                }
                else if (req.CreditosMinimos is not null && creditosAprobados < req.CreditosMinimos)
                {
                    motivo = $"Requiere al menos {req.CreditosMinimos} créditos aprobados";
                    break;
                }
            }

            resultado.Add(new CursoElegibleDto
            {
                IdCurso = pc.Curso.IdCurso,
                Codigo = pc.Curso.CodigoCurso,
                Nombre = pc.Curso.NombreCurso,
                Creditos = pc.Curso.CreditosCurso,
                RequiereLaboratorio = pc.Curso.RequiereLaboratorio,
                Ciclo = pc.Ciclo,
                CumpleRequisitos = motivo is null,
                MotivoNoElegible = motivo
            });
        }

        return resultado.OrderBy(c => c.Ciclo).ThenBy(c => c.Nombre).ToList();
    }

    public async Task<List<SeccionInscribibleDto>> GetSeccionesDisponiblesAsync(int idPeriodo, int idCurso)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var secciones = await db.Secciones.AsNoTracking()
            .Where(s => s.IdPeriodo == idPeriodo && s.IdCurso == idCurso && s.EstadoSeccion == EstadoConstantes.Activo)
            .Select(s => new
            {
                s.IdSeccion,
                s.CodigoSeccion,
                s.Jornada,
                s.CupoMaximo,
                CupoOcupado = s.DetallesAsignacion.Count(d => d.EstadoDetalle == EstadoConstantes.Activo),
                CatedraticoNombre = s.Catedratico!.NombresCatedratico + " " + s.Catedratico!.ApellidosCatedratico,
                SalonNombre = s.Salon!.NombreSalon,
                Horarios = s.HorariosSeccion.Select(h => new HorarioResumenDto { DiaSemana = h.DiaSemana, HoraInicio = h.HoraInicio, HoraFin = h.HoraFin }).ToList(),
                LabsRaw = s.SeccionesLaboratorio.Select(sl => new { sl.DiaSemana, sl.HoraInicio, sl.HoraFin, sl.CostoExtra }).ToList()
            })
            .ToListAsync();

        return secciones.Select(s => new SeccionInscribibleDto
        {
            IdSeccion = s.IdSeccion,
            Codigo = s.CodigoSeccion,
            Jornada = s.Jornada,
            CatedraticoNombre = s.CatedraticoNombre,
            SalonNombre = s.SalonNombre,
            CupoMaximo = s.CupoMaximo,
            CupoOcupado = s.CupoOcupado,
            Horarios = s.Horarios,
            HorariosLaboratorio = s.LabsRaw.Select(l => new HorarioResumenDto { DiaSemana = l.DiaSemana, HoraInicio = l.HoraInicio, HoraFin = l.HoraFin }).ToList(),
            CostoLaboratorio = s.LabsRaw.Select(l => l.CostoExtra).FirstOrDefault()
        }).ToList();
    }

    public async Task<ResultadoInscripcionDto> InscribirAsync(int idEstudiante, int idPeriodo, List<int> idsSecciones)
    {
        if (idsSecciones.Count == 0)
        {
            throw new ServiceException("Debe seleccionar al menos una sección.");
        }

        if (idsSecciones.Count > EstadoConstantes.MaxCursosPorPeriodo)
        {
            throw new ServiceException($"No puede inscribir más de {EstadoConstantes.MaxCursosPorPeriodo} cursos en un mismo período.");
        }

        await using var db = await dbFactory.CreateDbContextAsync();
        await using var transaccion = await db.Database.BeginTransactionAsync();

        var periodo = await db.PeriodosAcademicos.FirstOrDefaultAsync(p => p.IdPeriodo == idPeriodo)
            ?? throw new ServiceException("El período académico no existe.");
        if (!periodo.PermiteInscripcion || periodo.EstadoPeriodo != EstadoConstantes.Activo)
        {
            throw new ServiceException("Este período no admite inscripciones actualmente.");
        }

        var yaInscrito = await db.Inscripciones.AnyAsync(i =>
            i.IdEstudiante == idEstudiante && i.IdPeriodo == idPeriodo && i.EstadoInscripcion == EstadoConstantes.Activo);
        if (yaInscrito)
        {
            throw new ServiceException("El estudiante ya tiene una inscripción activa para este período.");
        }

        var estudiante = await db.Estudiantes.FirstOrDefaultAsync(e => e.IdEstudiante == idEstudiante)
            ?? throw new ServiceException("El estudiante no existe.");

        var secciones = await db.Secciones
            .Include(s => s.Curso)
            .Include(s => s.HorariosSeccion)
            .Include(s => s.SeccionesLaboratorio)
            .Include(s => s.DetallesAsignacion)
            .Where(s => idsSecciones.Contains(s.IdSeccion))
            .ToListAsync();

        if (secciones.Count != idsSecciones.Count)
        {
            throw new ServiceException("Una o más secciones seleccionadas ya no existen.");
        }

        // Re-validate capacity right before commit (avoids a race with the wizard's earlier read).
        foreach (var seccion in secciones)
        {
            var ocupados = seccion.DetallesAsignacion.Count(d => d.EstadoDetalle == EstadoConstantes.Activo);
            if (ocupados >= seccion.CupoMaximo)
            {
                throw new ServiceException($"La sección {seccion.CodigoSeccion} ya no tiene cupo disponible.");
            }
        }

        // Schedule-conflict check across the chosen sections (theory + lab blocks).
        var bloques = secciones.SelectMany(s => s.HorariosSeccion.Select(h => (Seccion: s.CodigoSeccion, h.DiaSemana, h.HoraInicio, h.HoraFin)))
            .Concat(secciones.SelectMany(s => s.SeccionesLaboratorio.Select(l => (Seccion: s.CodigoSeccion, l.DiaSemana, l.HoraInicio, l.HoraFin))))
            .ToList();

        for (var i = 0; i < bloques.Count; i++)
        {
            for (var j = i + 1; j < bloques.Count; j++)
            {
                var a = bloques[i];
                var b = bloques[j];
                if (a.Seccion == b.Seccion || a.DiaSemana != b.DiaSemana)
                {
                    continue;
                }

                if (a.HoraInicio < b.HoraFin && b.HoraInicio < a.HoraFin)
                {
                    throw new ServiceException($"Conflicto de horario entre {a.Seccion} y {b.Seccion} el día {a.DiaSemana}.");
                }
            }
        }

        // Cost computation: one lab fee per section (first block's rate), monthly tuition by credit.
        var creditosTotal = secciones.Sum(s => s.Curso!.CreditosCurso);
        var montoMensual = creditosTotal * PreciosConstantes.CostoPorCredito;
        var subtotalLaboratorios = secciones.Sum(s => s.SeccionesLaboratorio.Select(sl => sl.CostoExtra).FirstOrDefault());
        var totalPago = montoMensual + subtotalLaboratorios;

        var inscripcion = new Inscripcion
        {
            CostoInscripcion = PreciosConstantes.CostoInscripcion,
            MontoMensual = montoMensual,
            CicloInscrito = estudiante.CicloActual,
            EstadoSolvencia = EstadoConstantes.SolvenciaSolvente,
            EstadoInscripcion = EstadoConstantes.Activo,
            IdEstudiante = idEstudiante,
            IdPeriodo = idPeriodo
        };
        db.Inscripciones.Add(inscripcion);
        await db.SaveChangesAsync();

        var asignacion = new Data.Entities.Asignacion
        {
            SubtotalLaboratorios = subtotalLaboratorios,
            TotalPago = totalPago,
            EstadoAsignacion = EstadoConstantes.Activo,
            IdInscripcion = inscripcion.IdInscripcion
        };
        db.Asignaciones.Add(asignacion);
        await db.SaveChangesAsync();

        foreach (var seccion in secciones)
        {
            db.DetallesAsignacion.Add(new DetalleAsignacion
            {
                EstadoDetalle = EstadoConstantes.Activo,
                CostoLaboratorio = seccion.SeccionesLaboratorio.Select(sl => sl.CostoExtra).FirstOrDefault(),
                IdAsignacion = asignacion.IdAsignacion,
                IdSeccion = seccion.IdSeccion
            });
        }

        await db.SaveChangesAsync();
        await transaccion.CommitAsync();

        return new ResultadoInscripcionDto
        {
            IdInscripcion = inscripcion.IdInscripcion,
            IdAsignacion = asignacion.IdAsignacion,
            CostoInscripcion = inscripcion.CostoInscripcion,
            MontoMensual = montoMensual,
            SubtotalLaboratorios = subtotalLaboratorios,
            TotalPago = totalPago,
            SeccionesInscritas = secciones.Select(s => s.CodigoSeccion).ToList()
        };
    }

    public async Task<List<SeccionInscribibleDto>> GetMiHorarioAsync(int idEstudiante, int idPeriodo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var secciones = await db.DetallesAsignacion.AsNoTracking()
            .Where(d => d.EstadoDetalle == EstadoConstantes.Activo
                        && d.Asignacion!.Inscripcion!.IdEstudiante == idEstudiante
                        && d.Asignacion!.Inscripcion!.IdPeriodo == idPeriodo)
            .Select(d => d.Seccion!)
            .Select(s => new
            {
                s.IdSeccion,
                s.CodigoSeccion,
                s.Jornada,
                s.CupoMaximo,
                CupoOcupado = s.DetallesAsignacion.Count(d => d.EstadoDetalle == EstadoConstantes.Activo),
                CatedraticoNombre = s.Catedratico!.NombresCatedratico + " " + s.Catedratico!.ApellidosCatedratico,
                SalonNombre = s.Salon!.NombreSalon,
                Horarios = s.HorariosSeccion.Select(h => new HorarioResumenDto { DiaSemana = h.DiaSemana, HoraInicio = h.HoraInicio, HoraFin = h.HoraFin }).ToList(),
                LabsRaw = s.SeccionesLaboratorio.Select(sl => new { sl.DiaSemana, sl.HoraInicio, sl.HoraFin, sl.CostoExtra }).ToList()
            })
            .ToListAsync();

        return secciones.Select(s => new SeccionInscribibleDto
        {
            IdSeccion = s.IdSeccion,
            Codigo = s.CodigoSeccion,
            Jornada = s.Jornada,
            CatedraticoNombre = s.CatedraticoNombre,
            SalonNombre = s.SalonNombre,
            CupoMaximo = s.CupoMaximo,
            CupoOcupado = s.CupoOcupado,
            Horarios = s.Horarios,
            HorariosLaboratorio = s.LabsRaw.Select(l => new HorarioResumenDto { DiaSemana = l.DiaSemana, HoraInicio = l.HoraInicio, HoraFin = l.HoraFin }).ToList(),
            CostoLaboratorio = s.LabsRaw.Select(l => l.CostoExtra).FirstOrDefault()
        }).ToList();
    }

    private static async Task<HashSet<int>> CursosAprobadosAsync(AppDbContext db, int idEstudiante)
    {
        var ids = await db.DetallesAsignacion.AsNoTracking()
            .Where(d => d.Resultado == EstadoConstantes.ResultadoAprobado
                        && d.Asignacion!.Inscripcion!.IdEstudiante == idEstudiante)
            .Select(d => d.Seccion!.IdCurso)
            .ToListAsync();

        return ids.ToHashSet();
    }
}
