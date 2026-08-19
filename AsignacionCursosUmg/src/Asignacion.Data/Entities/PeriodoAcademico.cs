using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("periodo_academico")]
public class PeriodoAcademico
{
    [Key]
    [Column("id_periodo_academico")]
    public int IdPeriodo { get; set; }

    [Column("codigo_periodo_academico")]
    public required string CodigoPeriodo { get; set; }

    [Column("descripcion_periodo_academico")]
    public string? DescripcionPeriodo { get; set; }

    [Column("tipo_periodo_academico")]
    public required string TipoPeriodo { get; set; }

    [Column("fecha_inicio_periodo_academico")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin_periodo_academico")]
    public DateTime FechaFin { get; set; }

    [Column("permite_inscripcion_periodo_academico")]
    public bool PermiteInscripcion { get; set; }

    [Column("permite_asignacion_periodo_academico")]
    public bool PermiteAsignacion { get; set; }

    [Column("estado_periodo_academico")]
    public string EstadoPeriodo { get; set; } = "activo";

    public List<Seccion> Secciones { get; set; } = new();

    public List<Inscripcion> Inscripciones { get; set; } = new();
}
