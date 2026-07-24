using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("permiso")]
    public class Permiso
    {
        [Key]
        [Column("id_permiso")]
        public int IdPermiso { get; set; }
        [Column("nombre_permiso")]
        public required string NombrePermiso { get; set; }

        [Column("descripcion_permiso")]
        public required string DescripcionPermiso { get; set; }

        public List<RolPermiso> RolPermisos { get; set; } = new();
    }
}
