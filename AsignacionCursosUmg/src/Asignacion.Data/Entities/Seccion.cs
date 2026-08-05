using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asignacion.Data.Entities;

[Table("seccion")]
public class Seccion
{
    [Key]
    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [Column("codigo_seccion")]
    public required string CodigoSeccion { get; set; }

    [Column("jornada")]
    public required string Jornada { get; set; }

    [Column("cupo_maximo")]
    public int CupoMaximo { get; set; }

    [Column("estado_seccion")]
    public string EstadoSeccion { get; set; } = "activo";

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [ForeignKey(nameof(IdCurso))]
    public Curso? Curso { get; set; }

    [Column("id_periodo")]
    public int IdPeriodo { get; set; }

    [ForeignKey(nameof(IdPeriodo))]
    public PeriodoAcademico? PeriodoAcademico { get; set; }

    [Column("id_catedratico")]
    public int IdCatedratico { get; set; }

    [ForeignKey(nameof(IdCatedratico))]
    public Catedratico? Catedratico { get; set; }

    [Column("id_salon")]
    public int IdSalon { get; set; }

    [ForeignKey(nameof(IdSalon))]
    public Salon? Salon { get; set; }

    public List<HorarioSeccion> HorariosSeccion { get; set; } = new();

    public List<SeccionLaboratorio> SeccionesLaboratorio { get; set; } = new();

    public List<DetalleAsignacion> DetallesAsignacion { get; set; } = new();
}
