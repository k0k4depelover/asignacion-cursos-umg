using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("rol_permiso")]
    [PrimaryKey(nameof(IdRol), nameof(IdPermiso))] // clave compuesta
    public class RolPermiso
    {
        [Column("id_rol")]
        public int IdRol { get; set; }
        [ForeignKey(nameof(IdRol))]
        public Rol? Rol { get; set; }

        [Column("id_permiso")]
        public int IdPermiso { get; set; }
        [ForeignKey(nameof(IdPermiso))]
        public Permiso? Permiso { get; set; }
    }
}