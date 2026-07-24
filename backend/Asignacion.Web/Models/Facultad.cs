using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Asignacion.Web.Models
{
    [Table("facultad")]
    public class Facultad
    {
        [Key]
        [Column("id_facultad")]
        public int IdFacultad { get; set; }

        [Column("nombre_facultad")]
        public required string NombreFacultad { get; set; }

        [Column("descripcion_facultad")]
        public required string DescripcionFacultad { get; set; }

        [Column("estado_facultad")]
        public required string EstadoFacultad { get; set; }

        public List<Carrera> Carreras { get; set; } = new();
    }
}
