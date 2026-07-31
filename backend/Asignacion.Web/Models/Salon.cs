using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("salon")]
    public class Salon
    {
        [Key]
        [Column("id_salon")]
        public int IdSalon { get; set; }
        [Column("nombre_salon")]
        public required string NombreSalon { get; set; }

        [Column("codigo_salon")]
        public required string CodigoSalon { get; set; }

        [Column("estado_salon")]
        public required string EstadoSalon { get; set; }

        [Column("capacidad_salon")]
        public int CapacidadSalon { get; set; }

        [Column("tipo_espacio")]
        public required string TipoEspacio { get; set; }

        [Column("nivel_salon")]
        public int? NivelSalon { get; set; } // ← Nullable

        [Column("id_edificio")]
        public int IdEdificio { get; set; }

        [ForeignKey(nameof(IdEdificio))]
        public Edificio? Edificio { get; set; }

        public List<Laboratorio> Laboratorios { get; set; } = new();

        public List<Seccion> Secciones { get; set; } = new();
    }
}
