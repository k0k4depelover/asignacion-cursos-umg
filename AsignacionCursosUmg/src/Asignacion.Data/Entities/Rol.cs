using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("rol")]
public class Rol
{
    [Key]
    [Column("id_rol")]
    public int IdRol { get; set; }

    [Column("nombre_rol")]
    public required string NombreRol { get; set; }

    [Column("descripcion_rol")]
    public string? DescripcionRol { get; set; }

    [Column("estado_rol")]
    public string EstadoRol { get; set; } = "activo";

    public List<Usuario> Usuarios { get; set; } = new();

    public List<RolPermiso> RolPermisos { get; set; } = new();
}
