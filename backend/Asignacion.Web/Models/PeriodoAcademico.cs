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

        [Column("codigo_periodo")]
        public required string CodigoPeriodo { get; set; }

        [Column("descripcion_periodo")]
        public required string DescripcionPeriodo { get; set; }

        [Column("tipo_periodo")]
        public required string TipoPeriodo { get; set; }

        [Column ("fecha_inicio")]
        public required DateTime FechaInicio{ get; set; }
        [Column("fecha_fin")]
        public required DateTime FechaFin { get; set; }

        [Column("permite_inscripcion")]
        public required bool PermiteInscripcion { get; set; }

        [Column("permite_asignacion")]
        public required bool PermiteAsignacion { get; set; }

        [Column("estado_periodo")]
        public required string EstadoPeriodo { get; set; }

        public List<Seccion> Secciones { get; set; } = new();

        public List<Inscripcion> Inscripciones { get; set; } = new();

    }
}
