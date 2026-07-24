using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("seccion_laboratorio")]
    public class SeccionLaboratorio
    {
        [Key]
        [Column("id_seccion_laboratorio")]
        public int IdSeccionLaboratorio { get; set; }
        [Column("dia_semana")]
        public required string DiaSemana { get; set; }

        [Column("hora_inicio")]
        public string? HoraInicio { get; set; }

        [Column("costo_extra")]
        public required decimal CostoExtra { get; set; }

        [Column("id_seccion")]
        public required string IdSeccion { get; set; }

        [ForeignKey(nameof(IdSeccion))]
        public Seccion? Seccion { get; set; }


        [Column("id_laboratorio")]
        public int IdLaboratorio { get; set; }
        [ForeignKey(nameof(IdLaboratorio))]
        public Laboratorio? Laboratorio { get; set; }


    }
}
