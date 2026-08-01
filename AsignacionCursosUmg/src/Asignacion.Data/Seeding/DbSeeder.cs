using Microsoft.EntityFrameworkCore;

namespace Asignacion.Data.Seeding;

/// <summary>
/// Defensive self-heal for the demo accounts seeded by script_asignaciones_db.sql.
/// The SQL script remains the source of truth for seed data (roles, demo users,
/// catalog/infrastructure data) — this only rewrites a demo user's password hash
/// if it doesn't verify against the known demo password, in case an older/broken
/// copy of the seed script was applied.
/// </summary>
public static class DbSeeder
{
    private static readonly (string Correo, string Password)[] DemoCredenciales =
    [
        ("admin@umg.edu.gt", "Admin2025!"),
        ("estudiante@umg.edu.gt", "Est2025!"),
        ("catedratico@umg.edu.gt", "Cat2025!"),
    ];

    public static async Task EnsureDemoHashesAsync(AppDbContext db, CancellationToken ct = default)
    {
        foreach (var (correo, password) in DemoCredenciales)
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.CorreoLogin == correo, ct);
            if (usuario is null)
            {
                continue;
            }

            bool esValido;
            try
            {
                esValido = BCrypt.Net.BCrypt.Verify(password, usuario.ContrasenaHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                esValido = false;
            }

            if (!esValido)
            {
                usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(password);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
