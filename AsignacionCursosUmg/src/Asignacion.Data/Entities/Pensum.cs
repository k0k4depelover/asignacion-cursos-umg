using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("pensum")]
public class Pensum
{
    [Key]
    [Column("id_pensum")]
    public int IdPensum { get; set; }

    [Column("codigo_pensum")]
    public required string CodigoPensum { get; set; }

    [Column("anio_pensum")]
    public int AnioPensum { get; set; }

    [Column("jornada_pensum")]
    public required string JornadaPensum { get; set; }

    [Column("estado_pensum")]
    public string EstadoPensum { get; set; } = "activo";

    [Column("id_carrera_pensum")]
    public int IdCarrera { get; set; }

    [ForeignKey(nameof(IdCarrera))]
    public Carrera? Carrera { get; set; }

    public List<Estudiante> Estudiantes { get; set; } = new();

    public List<PensumCurso> PensumCursos { get; set; } = new();
}
