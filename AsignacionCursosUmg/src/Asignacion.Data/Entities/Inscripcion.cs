using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("inscripcion")]
public class Inscripcion
{
    [Key]
    [Column("id_inscripcion")]
    public int IdInscripcion { get; set; }

    [Column("fecha_inscripcion")]
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    [Column("costo_inscripcion")]
    public decimal CostoInscripcion { get; set; }

    [Column("monto_mensual")]
    public decimal MontoMensual { get; set; }

    [Column("ciclo_inscrito")]
    public int CicloInscrito { get; set; }

    [Column("estado_solvencia")]
    public string EstadoSolvencia { get; set; } = "solvente";

    [Column("estado_inscripcion")]
    public string EstadoInscripcion { get; set; } = "activo";

    [Column("id_estudiante_inscripcion")]
    public int IdEstudiante { get; set; }

    [ForeignKey(nameof(IdEstudiante))]
    public Estudiante? Estudiante { get; set; }

    [Column("id_periodo_academico_inscripcion")]
    public int IdPeriodo { get; set; }

    [ForeignKey(nameof(IdPeriodo))]
    public PeriodoAcademico? PeriodoAcademico { get; set; }

    public Asignacion? Asignacion { get; set; }
}
