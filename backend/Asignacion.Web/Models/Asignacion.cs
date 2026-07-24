using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("asignacion")]
    public class Asignacion
    {
        [Key]
        [Column("id_asignacion")]
        public int IdAsignacion { get; set; }
        [Column("fecha_asignacion")]
        public required DateTime FechaAsignacion { get; set; }

        [Column("subtotal_laboratorios")]
        public decimal? SubtotalLaboratorios { get; set; }

        [Column("total_pago")]
        public required decimal TotalPago { get; set; }

        [Column("estado_asignacion")]
        public required string EstadoAsignacion { get; set; }

        [Column("id_inscripcion")]
        public int IdInscripcion { get; set; }

        [ForeignKey(nameof(IdInscripcion))]
        public Inscripcion? Inscripcion { get; set; }

        public List<DetalleAsignacion>? DetallesAsignacion { get; set; } = new();



    }
}
