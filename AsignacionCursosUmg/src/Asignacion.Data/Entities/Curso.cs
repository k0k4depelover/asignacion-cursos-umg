using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("curso")]
public class Curso
{
    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("codigo_curso")]
    public required string CodigoCurso { get; set; }

    [Column("nombre_curso")]
    public required string NombreCurso { get; set; }

    [Column("creditos_curso")]
    public int CreditosCurso { get; set; }

    [Column("requiere_laboratorio_curso")]
    public bool RequiereLaboratorio { get; set; }

    [Column("estado_curso")]
    public string EstadoCurso { get; set; } = "activo";

    public List<PensumCurso> PensumCursos { get; set; } = new();

    public List<RequisitoCurso> RequisitoCursos { get; set; } = new();

    public List<Seccion> Secciones { get; set; } = new();
}
