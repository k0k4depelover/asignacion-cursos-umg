using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("carrera")]
public class Carrera
{
    [Key]
    [Column("id_carrera")]
    public int IdCarrera { get; set; }

    [Column("codigo_carrera")]
    public required string CodigoCarrera { get; set; }

    [Column("nombre_carrera")]
    public required string NombreCarrera { get; set; }

    [Column("total_ciclos_carrera")]
    public int TotalCiclos { get; set; }

    [Column("estado_carrera")]
    public string EstadoCarrera { get; set; } = "activo";

    [Column("id_facultad_carrera")]
    public int IdFacultad { get; set; }

    [ForeignKey(nameof(IdFacultad))]
    public Facultad? Facultad { get; set; }

    public List<Pensum> Pensums { get; set; } = new();
}
