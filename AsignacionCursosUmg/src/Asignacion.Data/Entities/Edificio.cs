using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("edificio")]
public class Edificio
{
    [Key]
    [Column("id_edificio")]
    public int IdEdificio { get; set; }

    [Column("codigo_edificio")]
    public required string CodigoEdificio { get; set; }

    [Column("nombre_edificio")]
    public required string NombreEdificio { get; set; }

    [Column("sede")]
    public required string Sede { get; set; }

    [Column("ubicacion")]
    public string? Ubicacion { get; set; }

    [Column("estado_edificio")]
    public string EstadoEdificio { get; set; } = "activo";

    public List<Salon> Salones { get; set; } = new();
}
