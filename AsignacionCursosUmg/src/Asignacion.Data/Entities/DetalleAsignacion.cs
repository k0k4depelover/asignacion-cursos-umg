using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("detalle_asignacion")]
public class DetalleAsignacion
{
    [Key]
    [Column("id_detalle_asignacion")]
    public int IdDetalleAsignacion { get; set; }

    [Column("estado_detalle")]
    public string EstadoDetalle { get; set; } = "activo";

    [Column("costo_laboratorio")]
    public decimal CostoLaboratorio { get; set; }

    [Column("nota_final")]
    public decimal? NotaFinal { get; set; }

    [Column("resultado")]
    public string? Resultado { get; set; }

    [Column("fecha_resultado")]
    public DateTime? FechaResultado { get; set; }

    [Column("id_asignacion")]
    public int IdAsignacion { get; set; }

    [ForeignKey(nameof(IdAsignacion))]
    public Asignacion? Asignacion { get; set; }

    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [ForeignKey(nameof(IdSeccion))]
    public Seccion? Seccion { get; set; }
}
