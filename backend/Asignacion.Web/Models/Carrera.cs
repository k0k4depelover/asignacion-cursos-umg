using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Asignacion.Web.Models
{
    [Table("carrera")]
    public class Carrera
    {
        [Key]
        [Column("id_carrera")]
        public int IdCarrera { get; set; }

        [Column("nombre_carrera")]
        public required string NombreCarrera { get; set; }

        [Column("codigo_carrera")]
        public required string CodigoCarrera { get; set; }

        [Column("total_ciclos")]
        public required int TotalCiclos { get; set; }

        [Column("estado_carrera")]
        public required string EstadoCarrera { get; set; }

        [Column("id_facultad")]
        public int IdFacultad { get; set; }

        [ForeignKey(nameof(IdFacultad))]
        public Facultad? Facultad { get; set; }

        public List<Pensum>? Pensums { get; set; }
        

    }
}
