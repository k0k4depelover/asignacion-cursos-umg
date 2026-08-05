using Asignacion.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<RolPermiso> RolPermisos => Set<RolPermiso>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Facultad> Facultades => Set<Facultad>();
    public DbSet<Carrera> Carreras => Set<Carrera>();
    public DbSet<Pensum> Pensums => Set<Pensum>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<PensumCurso> PensumCursos => Set<PensumCurso>();
    public DbSet<RequisitoCurso> RequisitoCursos => Set<RequisitoCurso>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Catedratico> Catedraticos => Set<Catedratico>();
    public DbSet<PeriodoAcademico> PeriodosAcademicos => Set<PeriodoAcademico>();
    public DbSet<Edificio> Edificios => Set<Edificio>();
    public DbSet<Salon> Salones => Set<Salon>();
    public DbSet<Laboratorio> Laboratorios => Set<Laboratorio>();
    public DbSet<Seccion> Secciones => Set<Seccion>();
    public DbSet<HorarioSeccion> HorariosSeccion => Set<HorarioSeccion>();
    public DbSet<SeccionLaboratorio> SeccionesLaboratorio => Set<SeccionLaboratorio>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
    public DbSet<Entities.Asignacion> Asignaciones => Set<Entities.Asignacion>();
    public DbSet<DetalleAsignacion> DetallesAsignacion => Set<DetalleAsignacion>();

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
        // Inscripcion 1:1 con Asignacion
        // ------------------------------------------------------------------
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Asignacion)
            .WithOne(a => a.Inscripcion)
            .HasForeignKey<Entities.Asignacion>(a => a.IdInscripcion);

        // ------------------------------------------------------------------
        // pensum_curso: evitar rutas de cascada ambiguas hacia Curso
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

        modelBuilder.Entity<PensumCurso>()
            .HasIndex(pc => new { pc.IdPensum, pc.IdCurso })
            .IsUnique();

        // ------------------------------------------------------------------
        // requisito_curso: dos caminos distintos hacia Curso
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
        // Seccion tiene 4 FKs distintas -> Restrict en todas
        // ------------------------------------------------------------------
        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.Curso)
            .WithMany(c => c.Secciones)
            .HasForeignKey(s => s.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seccion>()
            .HasOne(s => s.PeriodoAcademico)
            .WithMany(p => p.Secciones)
            .HasForeignKey(s => s.IdPeriodo)
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
            .WithMany(s => s.DetallesAsignacion)
            .HasForeignKey(d => d.IdSeccion)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleAsignacion>()
            .HasIndex(d => new { d.IdAsignacion, d.IdSeccion })
            .IsUnique();

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
        // Unicidades adicionales del esquema
        // ------------------------------------------------------------------
        modelBuilder.Entity<Usuario>().HasIndex(u => u.CorreoLogin).IsUnique();
        modelBuilder.Entity<Estudiante>().HasIndex(e => e.CarnetEstudiante).IsUnique();
        modelBuilder.Entity<Estudiante>().HasIndex(e => e.DpiEstudiante).IsUnique();
        modelBuilder.Entity<Catedratico>().HasIndex(c => c.CodigoCatedratico).IsUnique();
        modelBuilder.Entity<Catedratico>().HasIndex(c => c.DpiCatedratico).IsUnique();
        modelBuilder.Entity<Entities.Asignacion>().HasIndex(a => a.IdInscripcion).IsUnique();

        // ------------------------------------------------------------------
        // Precisión decimal explícita (evita el default decimal(65,30))
        // ------------------------------------------------------------------
        modelBuilder.Entity<SeccionLaboratorio>().Property(sl => sl.CostoExtra).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Inscripcion>().Property(i => i.CostoInscripcion).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Inscripcion>().Property(i => i.MontoMensual).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Entities.Asignacion>().Property(a => a.SubtotalLaboratorios).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Entities.Asignacion>().Property(a => a.TotalPago).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<DetalleAsignacion>().Property(d => d.CostoLaboratorio).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<DetalleAsignacion>().Property(d => d.NotaFinal).HasColumnType("decimal(5,2)");
    }
}
