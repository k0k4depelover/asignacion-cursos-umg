using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("laboratorio")]
    public class Laboratorio
    {
        [Key]
        [Column("id_laboratorio")]
        public int IdLaboratorio { get; set; }
        [Column("nombre_laboratorio")]
        public required string NombreLaboratorio { get; set; }

        [Column("descripcion_laboratorio")]
        public string? DescripcionLaboratorio { get; set; }

        [Column("estado_laboratorio")]
        public required string EstadoLaboratorio { get; set; }

        [Column("id_salon")]
        public required string IdSalon { get; set; }

        [ForeignKey(nameof(IdSalon))]
        public Salon? Salon { get; set; }

        public List<SeccionLaboratorio>? SeccionesLaboratorio { get; set; } = new();
    }
}
