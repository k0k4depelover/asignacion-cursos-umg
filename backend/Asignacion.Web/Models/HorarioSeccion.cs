using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("horario_seccion")]
    public class HorarioSeccion
    {
        [Key]
        [Column("id_horario")]
        public int IdHorario { get; set; }

        [Column("dia_semana_horario")]
        public required string DiaSemanaHorario { get; set; }

        [Column("hora_inicio")]
        public required TimeSpan HoraInicio { get; set; }
        [Column("hora_fin")]
        public required TimeSpan HoraFin { get; set; }

        [Column("tipo_sesion")]
        public required string TipoSesion { get; set; }
        [Column("id_seccion")]
        public required int IdSeccion { get; set; }
        [ForeignKey(nameof(IdSeccion))]
        public Seccion? Seccion { get; set; }

    }
}
