using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("horario_seccion")]
public class HorarioSeccion
{
    [Key]
    [Column("id_horario")]
    public int IdHorario { get; set; }

    [Column("dia_semana")]
    public required string DiaSemana { get; set; }

    [Column("hora_inicio")]
    public TimeSpan HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeSpan HoraFin { get; set; }

    [Column("tipo_sesion")]
    public required string TipoSesion { get; set; }

    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [ForeignKey(nameof(IdSeccion))]
    public Seccion? Seccion { get; set; }
}
