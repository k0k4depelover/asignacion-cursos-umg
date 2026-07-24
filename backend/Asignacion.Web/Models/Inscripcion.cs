using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
	[Table("inscripcion")]
	public class Inscripcion
	{
		[Key]
		[Column("id_inscripcion")]
		public int IdInscripcion { get; set; }

		[Column("fecha_inscripcion")]
		public required DateTime FechaInscripcion { get; set; }

		[Column("costo_inscripcion")]
		public decimal? CostoInscripcion { get; set; }

		[Column("estado_inscripcion")]
		public required string EstadoInscripcion { get; set; }

		[Column("monto_mensual")]
		public required decimal MontoMensual { get; set; }

		[Column("ciclo_inscrito")]
		public required string CicloInscrito { get; set; }

		[Column("estado_solvencia")]
		public required string EstadoSolvencia { get; set; }

		[Column("id_estudiante")]
		public required string IdEstudiante { get; set; }

		[ForeignKey(nameof(IdEstudiante))]
		public Estudiante? Estudiante { get; set; }

		[Column("id_periodo_academico")]
		public int IdPeriodoAcademico { get; set; }
		[ForeignKey(nameof(IdPeriodoAcademico))]
		public PeriodoAcademico? PeriodoAcademico { get; set; }

		public Asignacion Asignacion { get; set; }

	}
}
