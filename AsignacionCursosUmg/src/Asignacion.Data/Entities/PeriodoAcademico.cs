using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("periodo_academico")]
public class PeriodoAcademico
{
    [Key]
    [Column("id_periodo")]
    public int IdPeriodo { get; set; }

    [Column("codigo_periodo")]
    public required string CodigoPeriodo { get; set; }

    [Column("descripcion_periodo")]
    public string? DescripcionPeriodo { get; set; }

    [Column("tipo_periodo")]
    public required string TipoPeriodo { get; set; }

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }

    [Column("permite_inscripcion")]
    public bool PermiteInscripcion { get; set; }

    [Column("permite_asignacion")]
    public bool PermiteAsignacion { get; set; }

    [Column("estado_periodo")]
    public string EstadoPeriodo { get; set; } = "activo";

    public List<Seccion> Secciones { get; set; } = new();

    public List<Inscripcion> Inscripciones { get; set; } = new();
}
