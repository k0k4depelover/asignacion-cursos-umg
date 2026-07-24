using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Asignacion.Web.Models
{
    [Table("periodo_academico")]
    public class PeriodoAcademico
    {
        [Key]
        [Column("id_periodo_academico")]
        public int IdPeriodoAcademico { get; set; }

        [Column("codigo_periodo_academico")]
        public int CodigoPeriodoAcademico { get; set; }

        [Column("descripcion_periodo_academico")]
        public required string DescripcionPeriodoAcademico { get; set; }

        [Column("tipo_periodo_academico")]
        public required string TipoPeriodoAcademico { get; set; }

        [Column ("fecha_inicio_periodo_academico")]
        public required DateTime FechaInicioPeriodoAcademico { get; set; }
        [Column("fecha_fin_periodo_academico")]
        public required DateTime FechaFinPeriodoAcademico { get; set; }

        [Column("permite_inscripcion_periodo_academico")]
        public required bool PermiteInscripcionPeriodoAcademico { get; set; }

        [Column("permite_asignacion_periodo_academico")]
        public required bool PermiteAsignacionPeriodoAcademico { get; set; }

        [Column("estado_periodo_academico")]
        public required string EstadoPeriodoAcademico { get; set; }

        public List<Seccion> Secciones { get; set; } = new();

    }
}
