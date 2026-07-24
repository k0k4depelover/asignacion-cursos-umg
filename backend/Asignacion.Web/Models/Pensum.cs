using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Web.Models
{
    [Table("pensum")]
    public class Pensum
    {
        [Key]
        [Column("id_pensum")]
        public int IdPensum { get; set; }
        [Column("codigo_pensum")]
        public required string CodigoPensum { get; set; }

        [Column("anio_pensum")]
        public int AnioPensum { get; set; }

        [Column("estado_pensum")]
        public required string EstadoPensum { get; set; }

        [Column("jornada_pensum")]
        public required string JornadaPensum { get; set; }

        [Column("id_carrera")]
        public required string IdCarrera { get; set; }

        [ForeignKey(nameof(IdCarrera))]
        public Carrera? Carrera { get; set; }

        public List<PensumCurso>? PensumCursos { get; set; } = new();

    }
}
