using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Asignacion.Data;

/// <summary>
/// Design-time-only factory so `dotnet ef` tooling can resolve a context during
/// development (e.g. dbcontext info / scaffolding for inspection). Not part of the
/// runtime schema-creation path — the app never calls Migrate()/EnsureCreated();
/// script_asignaciones_db.sql is the single source of truth for the schema.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        const string designTimeConnectionString =
            "Server=localhost;Port=3306;Database=asignacion_cursos;User=root;Password=;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(designTimeConnectionString, ServerVersion.AutoDetect(designTimeConnectionString));

        return new AppDbContext(optionsBuilder.Options);
    }
}
