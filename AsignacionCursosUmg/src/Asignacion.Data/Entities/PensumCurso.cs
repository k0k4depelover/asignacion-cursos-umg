using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("pensum_curso")]
public class PensumCurso
{
    [Key]
    [Column("id_pensum_curso")]
    public int IdPensumCurso { get; set; }

    [Column("id_pensum")]
    public int IdPensum { get; set; }

    [ForeignKey(nameof(IdPensum))]
    public Pensum? Pensum { get; set; }

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [ForeignKey(nameof(IdCurso))]
    public Curso? Curso { get; set; }

    [Column("ciclo")]
    public int Ciclo { get; set; }

    [Column("es_obligatorio")]
    public bool EsObligatorio { get; set; } = true;

    public List<RequisitoCurso> RequisitoCursos { get; set; } = new();
}
