using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("seccion_laboratorio")]
public class SeccionLaboratorio
{
    [Key]
    [Column("id_seccion_laboratorio")]
    public int IdSeccionLaboratorio { get; set; }

    [Column("dia_semana_seccion_laboratorio")]
    public required string DiaSemana { get; set; }

    [Column("hora_inicio_seccion_laboratorio")]
    public TimeSpan HoraInicio { get; set; }

    [Column("hora_fin_seccion_laboratorio")]
    public TimeSpan HoraFin { get; set; }

    [Column("costo_extra_seccion_laboratorio")]
    public decimal CostoExtra { get; set; }

    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [ForeignKey(nameof(IdSeccion))]
    public Seccion? Seccion { get; set; }

    [Column("id_laboratorio")]
    public int IdLaboratorio { get; set; }

    [ForeignKey(nameof(IdLaboratorio))]
    public Laboratorio? Laboratorio { get; set; }
}
