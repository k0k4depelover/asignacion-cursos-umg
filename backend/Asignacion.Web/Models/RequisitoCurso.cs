using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("requisito_curso")]
    public class RequisitoCurso
    {
        [Key]
        [Column("id_requisito_curso")]
        public int IdRequisitoCurso { get; set; }

        [Column("id_pensum_curso")]
        public required int IdPensumCurso { get; set; }
        [ForeignKey(nameof(IdPensumCurso))]
        public PensumCurso? PensumCurso { get; set; }

        [Column("tipo_requisito")]
        public required string TipoRequisito { get; set; }

        [Column("id_curso_requerido")]
        public int IdCursoRequerido { get; set; }
        [ForeignKey(nameof(IdCursoRequerido))]
        public Curso? CursoRequerido { get; set; }

        [Column("creditos_minimos")]
        public int CreditosMinimos { get; set; }

        [Column("descripcion_requisito")]
        public required string DescripcionRequisito { get; set; }



    }
}
