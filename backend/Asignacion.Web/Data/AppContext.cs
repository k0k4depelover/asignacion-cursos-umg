using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Models;

namespace Asignacion.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<RolPermiso> RolPermisos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Facultad> Facultades { get; set; }
    public DbSet<Carrera> Carreras { get; set; }
    public DbSet<Pensum> Pensums { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<PensumCurso> PensumCursos { get; set; }
    public DbSet<RequisitoCurso> RequisitoCursos { get; set; }
    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Catedratico> Catedraticos { get; set; }
    public DbSet<PeriodoAcademico> PeriodosAcademicos { get; set; }
    public DbSet<Edificio> Edificios { get; set; }
    public DbSet<Salon> Salones { get; set; }
    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Seccion> Secciones { get; set; }
    public DbSet<HorarioSeccion> HorariosSeccion { get; set; }
    public DbSet<SeccionLaboratorio> SeccionesLaboratorio { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }
    public DbSet<Asignacion> Asignaciones { get; set; }
    public DbSet<DetalleAsignacion> DetallesAsignacion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ------------------------------------------------------------------
        // RolPermiso: clave primaria compuesta (N:M puro, sin columnas propias)
        // ------------------------------------------------------------------
        modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => new { rp.IdRol, rp.IdPermiso });

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany(r => r.RolPermisos)
            .HasForeignKey(rp => rp.IdRol);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany(p => p.RolPermisos)
            .HasForeignKey(rp => rp.IdPermiso);

        // ------------------------------------------------------------------
        // Usuario 1:1 opcional con Estudiante y con Catedratico
        // (un usuario puede o no tener un perfil de cada tipo, según su rol)
        // ------------------------------------------------------------------
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Estudiante)
            .WithOne(e => e.Usuario)
            .HasForeignKey<Estudiante>(e => e.IdUsuario);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Catedratico)
            .WithOne(c => c.Usuario)
            .HasForeignKey<Catedratico>(c => c.IdUsuario);

        // ------------------------------------------------------------------
        // Inscripcion 1:1 con Asignacion (una inscripción genera una asignación)
        // ------------------------------------------------------------------
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Asignacion)
            .WithOne(a => a.Inscripcion)
            .HasForeignKey<Asignacion>(a => a.IdInscripcion);

        // ------------------------------------------------------------------
        // pensum_curso: N:M con columnas propias -> evitar borrado en cascada
        // múltiple (MySQL no permite más de un camino de cascada hacia la
        // misma tabla en algunos casos); lo dejamos en Restrict por seguridad
        // ------------------------------------------------------------------
        modelBuilder.Entity<PensumCurso>()
            .HasOne(pc => pc.Pensum)
            .WithMany(p => p.PensumCursos)
            .HasForeignKey(pc => pc.IdPensum)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PensumCurso>()
            .HasOne(pc => pc.Curso)
            .WithMany(c => c.PensumCursos)
            .HasForeignKey(pc => pc.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);

        // ------------------------------------------------------------------
        // requisito_curso: dos caminos distintos hacia Curso
        // (uno indirecto vía PensumCurso, otro directo vía CursoRequerido)
        // Restrict evita ciclos de cascada ambiguos
        // ------------------------------------------------------------------
        modelBuilder.Entity<RequisitoCurso>()
            .HasOne(rc => rc.CursoRequerido)
            .WithMany(c => c.RequisitoCursos)
            .HasForeignKey(rc => rc.IdCursoRequerido)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RequisitoCurso>()
            .HasOne(rc => rc.PensumCurso)
            .WithMany(pc => pc.RequisitoCursos)
            .HasForeignKey(rc => rc.IdPensumCurso)
            .OnDelete(DeleteBehavior.Restrict);

        // ------------------------------------------------------------------
        // Seccion tiene 4 FKs distintas -> Restrict en todas para que
        // borrar un Curso/Catedratico/Salon/Periodo no arrastre cascadas
        // cruzadas que MySQL rechace al crear las constraints
        // ------------------------------------------------------------------
        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.Curso)
            .WithMany(c => c.Secciones)
            .HasForeignKey(s => s.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.PeriodoAcademico)
            .WithMany(p => p.Secciones)
            .HasForeignKey(s => s.IdPeriodoAcademico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.Catedratico)
            .WithMany(c => c.Secciones)
            .HasForeignKey(s => s.IdCatedratico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.Salon)
            .WithMany(sa => sa.Secciones)
            .HasForeignKey(s => s.IdSalon)
            .OnDelete(DeleteBehavior.Restrict);

        // ------------------------------------------------------------------
        // detalle_asignacion: dos FKs (Asignacion, Seccion) -> Restrict
        // ------------------------------------------------------------------
        modelBuilder.Entity<DetalleAsignacion>()
            .HasOne(d => d.Asignacion)
            .WithMany(a => a.DetallesAsignacion)
            .HasForeignKey(d => d.IdAsignacion)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleAsignacion>()
            .HasOne(d => d.Seccion)
            .WithMany(s => s.DetalleAsignaciones)
            .HasForeignKey(d => d.IdSeccion)
            .OnDelete(DeleteBehavior.Restrict);

        // ------------------------------------------------------------------
        // seccion_laboratorio: dos FKs (Seccion, Laboratorio) -> Restrict
        // ------------------------------------------------------------------
        modelBuilder.Entity<SeccionLaboratorio>()
            .HasOne(sl => sl.Seccion)
            .WithMany(s => s.SeccionesLaboratorio)
            .HasForeignKey(sl => sl.IdSeccion)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SeccionLaboratorio>()
            .HasOne(sl => sl.Laboratorio)
            .WithMany(l => l.SeccionesLaboratorio)
            .HasForeignKey(sl => sl.IdLaboratorio)
            .OnDelete(DeleteBehavior.Restrict);

        // ------------------------------------------------------------------
        // Precisión decimal explícita (evita el default decimal(65,30))
        // ------------------------------------------------------------------
        modelBuilder.Entity<SeccionLaboratorio>().Property(sl => sl.CostoExtra).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Inscripcion>().Property(i => i.CostoInscripcion).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Inscripcion>().Property(i => i.MontoMensual).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Asignacion>().Property(a => a.SubtotalAsignacion).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Asignacion>().Property(a => a.TotalPago).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<DetalleAsignacion>().Property(d => d.CostoLaboratorio).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<DetalleAsignacion>().Property(d => d.NotaFinal).HasColumnType("decimal(5,2)");
    }
}