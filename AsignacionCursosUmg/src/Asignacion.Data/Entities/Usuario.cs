using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("usuario")]
public class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre_usuario")]
    public required string NombreUsuario { get; set; }

    [Column("correo_login_usuario")]
    public required string CorreoLogin { get; set; }

    [Column("correo_recuperacion_usuario")]
    public string? CorreoRecuperacion { get; set; }

    [Column("contrasena_hash_usuario")]
    public required string ContrasenaHash { get; set; }

    [Column("tiene_pass_temporal_usuario")]
    public bool TienePassTemporal { get; set; }

    [Column("estado_usuario")]
    public string EstadoUsuario { get; set; } = "activo";

    [Column("fecha_registro_usuario")]
    public DateTime FechaRegistroUsuario { get; set; } = DateTime.Now;

    [Column("id_rol_usuario")]
    public int IdRol { get; set; }

    [ForeignKey(nameof(IdRol))]
    public Rol? Rol { get; set; }

    public Estudiante? Estudiante { get; set; }

    public Catedratico? Catedratico { get; set; }
}
