using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("edificio")]
    public class Edificio
    {
        [Key]
        [Column("id_edificio")]
        public int IdEdificio { get; set; }

        [Column("nombre_edificio")]
        public required string NombreEdificio { get; set; }

        [Column("codigo_edificio")]
        public required string CodigoEdificio { get; set; }

        [Column("sede")]
        public required string SedeEdificio { get; set; }

        [Column("ubicacion")]
        public required string UbicacionEdificio { get; set; }

        [Column("estado_edificio")]
        public required string EstadoEdificio { get; set; }

        public List<Salon> Salones { get; set; } = new();
    }
}
