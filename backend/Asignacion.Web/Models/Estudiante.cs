using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("estudiante")]
    public class Estudiante
    {
        [Key]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [Column("nombres_estudiante")]
        public required string NombresEstudiante { get; set; }

        [Column("apellidos_estudiante")]
        public required string ApellidosEstudiante { get; set; }

        [Column("carnet_estudiante")]
        public required string CarnetEstudiante { get; set; }

        [Column("dpi_estudiante")]
        public required string DpiEstudiante { get; set; }

        [Column("fecha_nacimiento_estudiante")]
        public required DateTime FechaNacimientoEstudiante { get; set; }

        [Column("direccion_estudiante")]
        public required string DireccionEstudiante { get; set; }

        [Column("telefono_estudiante")]
        public required string TelefonoEstudiante { get; set; }

        [Column ("ciclo_actual")]
        public int CicloEstudiante { get; set; }

        [Column("estado_estudiante")]
        public required string EstadoEstudiante { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        [Column("id_pensum")]
        public int IdPensum { get; set; }
        [ForeignKey(nameof(IdPensum))]
        public Pensum? Pensum { get; set; }


        public List<Inscripcion>? Inscripciones { get; set; } = new();

    }
}
