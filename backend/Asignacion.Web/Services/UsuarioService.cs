using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Usuario;
// No usar using BCrypt.Net;

namespace Asignacion.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosUsuariosAsync()
        {
            return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        public async Task<Usuario> CrearUsuarioAsync(CreateUserDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.CorreoLoginUsuario == dto.CorreoLogin);
            if (existe)
                throw new InvalidOperationException("El correo de login ya esta registrado.");

            // Uso del nombre completo
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                CorreoLoginUsuario = dto.CorreoLogin,
                CorreoRecuperacionUsuario = dto.CorreoRecuperacion ?? string.Empty,
                ContrasenaHash = passwordHash,
                TienePassTemporal = dto.TienePassTemporal,
                EstadoUsuario = dto.EstadoUsuario,
                FechaRegistroUsuario = DateTime.Now,
                IdRol = dto.IdRol
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ActualizarUsuarioAsync(int idUsuario, UpdateUserDto dto)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(idUsuario);
            if (usuarioExistente == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.NombreUsuario))
                usuarioExistente.NombreUsuario = dto.NombreUsuario;

            if (!string.IsNullOrWhiteSpace(dto.CorreoLogin))
            {
                var existe = await _context.Usuarios.AnyAsync(u => u.CorreoLoginUsuario == dto.CorreoLogin && u.IdUsuario != idUsuario);
                if (existe)
                    throw new InvalidOperationException("El correo de login ya esta registrado por otro usuario.");
                usuarioExistente.CorreoLoginUsuario = dto.CorreoLogin;
            }

            if (dto.CorreoRecuperacion != null)
                usuarioExistente.CorreoRecuperacionUsuario = dto.CorreoRecuperacion;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuarioExistente.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                usuarioExistente.TienePassTemporal = dto.TienePassTemporal ?? false;
            }

            if (dto.EstadoUsuario != null)
                usuarioExistente.EstadoUsuario = dto.EstadoUsuario;

            if (dto.IdRol.HasValue)
                usuarioExistente.IdRol = dto.IdRol.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarUsuarioAsync(int idUsuario)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(idUsuario);
            if (usuarioExistente == null) return false;

            _context.Usuarios.Remove(usuarioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}