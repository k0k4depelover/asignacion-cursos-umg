using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("salon")]
public class Salon
{
    [Key]
    [Column("id_salon")]
    public int IdSalon { get; set; }

    [Column("codigo_salon")]
    public required string CodigoSalon { get; set; }

    [Column("nombre_salon")]
    public required string NombreSalon { get; set; }

    [Column("capacidad_salon")]
    public int CapacidadSalon { get; set; }

    [Column("tipo_espacio_salon")]
    public required string TipoEspacio { get; set; }

    [Column("nivel_salon")]
    public int NivelSalon { get; set; }

    [Column("estado_salon")]
    public string EstadoSalon { get; set; } = "activo";

    [Column("id_edificio_salon")]
    public int IdEdificio { get; set; }

    [ForeignKey(nameof(IdEdificio))]
    public Edificio? Edificio { get; set; }

    public List<Laboratorio> Laboratorios { get; set; } = new();

    public List<Seccion> Secciones { get; set; } = new();
}
