using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Asignacion.Web.Models
{
    [Table("periodo_academico")]
    public class PeriodoAcademico
    {
        [Key]
        [Column("id_periodo")]
        public int IdPeriodo { get; set; }

        [Column("codigo_periodo_academico")]
        public required string CodigoPeriodo { get; set; }

        [Column("descripcion_periodo_academico")]
        public required string DescripcionPeriodo { get; set; }

        [Column("tipo_periodo_academico")]
        public required string TipoPeriodo { get; set; }

        [Column ("fecha_inicio_academico")]
        public required DateTime FechaInicio{ get; set; }
        [Column("fecha_fin_academico")]
        public required DateTime FechaFin { get; set; }

        [Column("permite_inscripcion_academico")]
        public required bool PermiteInscripcion { get; set; }

        [Column("permite_asignacion_academico")]
        public required bool PermiteAsignacion { get; set; }

        [Column("estado_periodo_academico")]
        public required string EstadoPeriodo { get; set; }

        public List<Seccion> Secciones { get; set; } = new();

        public List<Inscripcion> Inscripciones { get; set; } = new();

    }
}
