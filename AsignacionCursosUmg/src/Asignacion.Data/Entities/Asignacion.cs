using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("asignacion")]
public class Asignacion
{
    [Key]
    [Column("id_asignacion")]
    public int IdAsignacion { get; set; }

    [Column("fecha_asignacion")]
    public DateTime FechaAsignacion { get; set; } = DateTime.Now;

    [Column("subtotal_laboratorios")]
    public decimal SubtotalLaboratorios { get; set; }

    [Column("total_pago")]
    public decimal TotalPago { get; set; }

    [Column("estado_asignacion")]
    public string EstadoAsignacion { get; set; } = "activo";

    [Column("id_inscripcion_asignacion")]
    public int IdInscripcion { get; set; }

    [ForeignKey(nameof(IdInscripcion))]
    public Inscripcion? Inscripcion { get; set; }

    public List<DetalleAsignacion> DetallesAsignacion { get; set; } = new();
}
