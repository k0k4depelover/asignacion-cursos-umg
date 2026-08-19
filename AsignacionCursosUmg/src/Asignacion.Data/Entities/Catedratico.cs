using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("catedratico")]
public class Catedratico
{
    [Key]
    [Column("id_catedratico")]
    public int IdCatedratico { get; set; }

    [Column("codigo_catedratico")]
    public required string CodigoCatedratico { get; set; }

    [Column("dpi_catedratico")]
    public required string DpiCatedratico { get; set; }

    [Column("nombres_catedratico")]
    public required string NombresCatedratico { get; set; }

    [Column("apellidos_catedratico")]
    public required string ApellidosCatedratico { get; set; }

    [Column("telefono_catedratico")]
    public string? TelefonoCatedratico { get; set; }

    [Column("profesion_catedratico")]
    public string? ProfesionCatedratico { get; set; }

    [Column("estado_catedratico")]
    public string EstadoCatedratico { get; set; } = "activo";

    [Column("id_usuario_catedratico")]
    public int IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario? Usuario { get; set; }

    public List<Seccion> Secciones { get; set; } = new();
}
