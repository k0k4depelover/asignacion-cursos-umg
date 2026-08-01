using System.Security.Cryptography;
using Asignacion.Data;
using Asignacion.Data.Common;
using Asignacion.Data.Entities;
using Asignacion.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Asignacion.Services.Personas;

public class UsuarioDto
{
    public int Id { get; set; }
    public required string NombreUsuario { get; set; }
    public required string CorreoLogin { get; set; }
    public string? CorreoRecuperacion { get; set; }
    public string Estado { get; set; } = EstadoConstantes.Activo;
    public bool TienePassTemporal { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int IdRol { get; set; }
    public string RolNombre { get; set; } = "";
}

public interface IUsuarioService
{
    Task<List<UsuarioDto>> GetAllAsync(bool incluirInactivos = false);
    Task<UsuarioDto> CreateAsync(UsuarioDto dto, string password);
    Task UpdateAsync(UsuarioDto dto);
    Task SetEstadoAsync(int id, bool activo);

    /// <summary>Generates and stores a new temporary password, returning it in the clear
    /// exactly once so the admin can hand it to the user (never persisted as plaintext).</summary>
    Task<string> ResetearPasswordAsync(int id);
}

public class UsuarioService(IDbContextFactory<AppDbContext> dbFactory) : IUsuarioService
{
    public async Task<List<UsuarioDto>> GetAllAsync(bool incluirInactivos = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var query = db.Usuarios.AsNoTracking().AsQueryable();
        if (!incluirInactivos)
        {
            query = query.Where(u => u.EstadoUsuario == EstadoConstantes.Activo);
        }

        return await query
            .OrderBy(u => u.NombreUsuario)
            .Select(u => new UsuarioDto
            {
                Id = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                CorreoLogin = u.CorreoLogin,
                CorreoRecuperacion = u.CorreoRecuperacion,
                Estado = u.EstadoUsuario,
                TienePassTemporal = u.TienePassTemporal,
                FechaRegistro = u.FechaRegistroUsuario,
                IdRol = u.IdRol,
                RolNombre = u.Rol!.NombreRol
            })
            .ToListAsync();
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioDto dto, string password)
    {
        return await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                CorreoLogin = dto.CorreoLogin,
                CorreoRecuperacion = dto.CorreoRecuperacion,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(password),
                TienePassTemporal = dto.TienePassTemporal,
                EstadoUsuario = dto.Estado,
                IdRol = dto.IdRol
            };
            db.Usuarios.Add(entidad);
            await db.SaveChangesAsync();
            dto.Id = entidad.IdUsuario;
            dto.FechaRegistro = entidad.FechaRegistroUsuario;
            return dto;
        }, "Usuario");
    }

    public async Task UpdateAsync(UsuarioDto dto)
    {
        await DbExceptionTranslator.RunAsync(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var entidad = await db.Usuarios.FindAsync(dto.Id)
                ?? throw new ServiceException("El usuario ya no existe.");
            entidad.NombreUsuario = dto.NombreUsuario;
            entidad.CorreoLogin = dto.CorreoLogin;
            entidad.CorreoRecuperacion = dto.CorreoRecuperacion;
            entidad.EstadoUsuario = dto.Estado;
            entidad.IdRol = dto.IdRol;
            await db.SaveChangesAsync();
        }, "Usuario");
    }

    public async Task SetEstadoAsync(int id, bool activo)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Usuarios.FindAsync(id)
            ?? throw new ServiceException("El usuario ya no existe.");
        entidad.EstadoUsuario = activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo;
        await db.SaveChangesAsync();
    }

    public async Task<string> ResetearPasswordAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entidad = await db.Usuarios.FindAsync(id)
            ?? throw new ServiceException("El usuario ya no existe.");

        var temporal = GenerarPasswordTemporal();
        entidad.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(temporal);
        entidad.TienePassTemporal = true;
        await db.SaveChangesAsync();

        return temporal;
    }

    private static string GenerarPasswordTemporal()
    {
        const string alfabeto = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$";
        Span<byte> bytes = stackalloc byte[12];
        RandomNumberGenerator.Fill(bytes);
        var chars = new char[12];
        for (var i = 0; i < bytes.Length; i++)
        {
            chars[i] = alfabeto[bytes[i] % alfabeto.Length];
        }

        return new string(chars);
    }
}
