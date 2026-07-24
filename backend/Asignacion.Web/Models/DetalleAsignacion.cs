using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("detalle_asignacion")]
    public class DetalleAsignacion
    {
        [Key]
        [Column("id_detalle_asignacion")]
        public int IdDetalleAsignacion { get; set; }
        [Column("estado_detalle")]
        public required string EstadoDetalle { get; set; }

        [Column("costo_laboratorio")]
        public decimal? CostoLaboratorio { get; set; }

        [Column("nota_final")]
        public required decimal NotaFinal { get; set; }

        [Column("resultado")]
        public required string Resultado { get; set; }

        [Column("fecha_resultado")]
        public required DateTime FechaResultado { get; set; }

        [Column("id_asignacion")]
        public int IdAsignacion { get; set; }

        [ForeignKey(nameof(IdAsignacion))]
        public Asignacion? Asignacion { get; set; }

        [Column("id_seccion")]
        public int IdSeccion { get; set; }
        [ForeignKey(nameof(IdSeccion))]
        public Seccion? Seccion { get; set; }


    }
}
