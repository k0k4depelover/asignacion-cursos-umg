using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("requisito_curso")]
public class RequisitoCurso
{
    [Key]
    [Column("id_requisito")]
    public int IdRequisito { get; set; }

    [Column("id_pensum_curso_requisito")]
    public int IdPensumCurso { get; set; }

    [ForeignKey(nameof(IdPensumCurso))]
    public PensumCurso? PensumCurso { get; set; }

    [Column("tipo_requisito")]
    public required string TipoRequisito { get; set; }

    [Column("id_curso_requisito")]
    public int IdCursoRequerido { get; set; }

    [ForeignKey(nameof(IdCursoRequerido))]
    public Curso? CursoRequerido { get; set; }

    [Column("creditos_minimos_requisito")]
    public int? CreditosMinimos { get; set; }

    [Column("descripcion_requisito")]
    public string? DescripcionRequisito { get; set; }
}
