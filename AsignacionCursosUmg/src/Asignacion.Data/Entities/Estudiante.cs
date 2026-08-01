using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("estudiante")]
public class Estudiante
{
    [Key]
    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("carnet_estudiante")]
    public required string CarnetEstudiante { get; set; }

    [Column("dpi_estudiante")]
    public required string DpiEstudiante { get; set; }

    [Column("nombres_estudiante")]
    public required string NombresEstudiante { get; set; }

    [Column("apellidos_estudiante")]
    public required string ApellidosEstudiante { get; set; }

    [Column("fecha_nacimiento")]
    public DateTime FechaNacimiento { get; set; }

    [Column("direccion_estudiante")]
    public string? DireccionEstudiante { get; set; }

    [Column("telefono_estudiante")]
    public string? TelefonoEstudiante { get; set; }

    [Column("ciclo_actual")]
    public int CicloActual { get; set; } = 1;

    [Column("estado_estudiante")]
    public string EstadoEstudiante { get; set; } = "activo";

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario? Usuario { get; set; }

    [Column("id_pensum")]
    public int IdPensum { get; set; }

    [ForeignKey(nameof(IdPensum))]
    public Pensum? Pensum { get; set; }

    public List<Inscripcion> Inscripciones { get; set; } = new();
}
