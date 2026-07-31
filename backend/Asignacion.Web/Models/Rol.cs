using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("rol")]
    public class Rol
    {
        [Key]
        [Column("id_rol")]
        public int IdRol { get; set; }

        [Column("nombre_rol")]
        public required string NombreRol { get; set; }

        [Column("descripcion_rol")]
        public string? DescripcionRol { get; set; } // ← Nullable

        [Column("estado_rol")]
        public required string EstadoRol { get; set; }

        public List<Usuario> Usuarios { get; set; } = new();
        public List<RolPermiso> RolPermisos { get; set; } = new();
    }
}
