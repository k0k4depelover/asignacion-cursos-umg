using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Asignacion.Web.Models
{
    [Table("seccion")]
    public class Seccion 
    {
        [Key]
        [Column("id_seccion")]
        public int IdSeccion { get; set; }

        [Column("codigo_seccion")]
        public required string CodigoSeccion { get; set; }

        [Column("jornada_seccion")]
        public required string JornadaSeccion { get; set; }

        [Column("cupo_maximo_seccion")]
        public int CupoMaximoSeccion { get; set; }

        [Column("estado_seccion")]
        public required string EstadoSeccion { get; set; }

        [Column("id_curso")]
        public int IdCurso { get; set; }
        [ForeignKey(nameof(IdCurso))]
        public Curso? Curso { get; set; }

        [Column("id_periodo_academico")]
        public int IdPeriodoAcademico { get; set; }
        [ForeignKey(nameof(IdPeriodoAcademico))]
        public PeriodoAcademico? PeriodoAcademico { get; set; }


        [Column("id_catedratico")]
        public int IdCatedratico { get; set; }
        [ForeignKey(nameof(IdCatedratico))]
        public Catedratico? Catedratico { get; set; }   


        [Column("id_salon")]
        public int IdSalon { get; set; }
        [ForeignKey(nameof(IdSalon))]
        public Salon? Salon { get; set; }

        public List<SeccionLaboratorio>? SeccionesLaboratorio { get; set; } = new();

        public List<DetalleAsignacion>? DetalleAsignaciones { get; set; } = new();

        public List<HorarioSeccion>? HorarioSecciones { get; set; } = new();

    }
}
