using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("catedratico")]
    public class Catedratico
    {
        [Key]
        [Column("id_catedratico")]
        public int IdCatedratico { get; set; }

        [Column("codigo_catedratico")]
        public int CodigoCatedratico { get; set; }

        [Column("dpi_catedratico")]
        public required string DpiCatedratico { get; set; }

        [Column("nombres_catedratico")]
        public required string NombresCatedratico { get; set; }

        [Column("apellidos_catedratico")]
        public required string ApellidosCatedratico { get; set; }

        [Column("telefono_catedratico")]
        public string? TelefonoCatedratico { get; set; } // ← Nullable

        [Column("profesion_catedratico")]
        public string? ProfesionCatedratico { get; set; } // ← Nullable

        [Column("estado_catedratico")]
        public required string EstadoCatedratico { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        public List<Seccion> Secciones { get; set; } = new();
    }
}