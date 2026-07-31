using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre_usuario")]
        public required string NombreUsuario { get; set; }

        [Column("correo_login")]
        public required string CorreoLoginUsuario { get; set; }

        [Column("correo_recuperacion")]
        public string? CorreoRecuperacionUsuario { get; set; } // ← Nullable

        [Column("contrasena_hash")]
        public required string ContrasenaHash { get; set; }

        [Column("tiene_pass_temporal")]
        public bool TienePassTemporal { get; set; } = false;

        [Column("estado_usuario")]
        public required string EstadoUsuario { get; set; }

        [Column("fecha_registro_usuario")]
        public DateTime FechaRegistroUsuario { get; set; } = DateTime.Now;

        [Column("id_rol")]
        public int IdRol { get; set; }

        [ForeignKey(nameof(IdRol))]
        public Rol? Rol { get; set; }

        public Estudiante? Estudiante { get; set; }
        public Catedratico? Catedratico { get; set; }
    }
}