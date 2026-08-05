using Asignacion.Data;
using Asignacion.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Auth;

public class AuthService(IDbContextFactory<AppDbContext> dbFactory) : IAuthService
{
    public async Task<SesionUsuario?> LoginAsync(string correo, string password)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estudiante)
            .Include(u => u.Catedratico)
            .FirstOrDefaultAsync(u => u.CorreoLogin == correo);

        if (usuario is null || usuario.Rol is null)
        {
            return null;
        }

        if (usuario.EstadoUsuario != EstadoConstantes.Activo)
        {
            return null;
        }

        bool passwordValida;
        try
        {
            passwordValida = BCrypt.Net.BCrypt.Verify(password, usuario.ContrasenaHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            passwordValida = false;
        }

        if (!passwordValida)
        {
            return null;
        }

        return new SesionUsuario(
            usuario.IdUsuario,
            usuario.NombreUsuario,
            usuario.CorreoLogin,
            usuario.Rol.NombreRol,
            usuario.Estudiante?.IdEstudiante,
            usuario.Catedratico?.IdCatedratico);
    }
}
