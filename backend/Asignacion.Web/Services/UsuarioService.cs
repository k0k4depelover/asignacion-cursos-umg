using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Usuario;
using BCrypt.Net;

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
            return await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        public async Task<Usuario> CrearUsuarioAsync(CreateUserDto dto)
        {
            // Verificar que el correo no exista
            var existe = await _context.Usuarios
                .AnyAsync(u => u.CorreoLoginUsuario == dto.CorreoLogin);
            if (existe)
                throw new InvalidOperationException("El correo de login ya está registrado.");

            // Hashear la contraseña con BCrypt
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
            if (usuarioExistente == null)
                return false;

            // Actualizar solo los campos que vienen en el DTO
            if (!string.IsNullOrWhiteSpace(dto.NombreUsuario))
                usuarioExistente.NombreUsuario = dto.NombreUsuario;

            if (!string.IsNullOrWhiteSpace(dto.CorreoLogin))
            {
                // Verificar que el nuevo correo no esté en uso por otro usuario
                var existe = await _context.Usuarios
                    .AnyAsync(u => u.CorreoLoginUsuario == dto.CorreoLogin && u.IdUsuario != idUsuario);
                if (existe)
                    throw new InvalidOperationException("El correo de login ya está registrado por otro usuario.");
                usuarioExistente.CorreoLoginUsuario = dto.CorreoLogin;
            }

            if (dto.CorreoRecuperacion != null)
                usuarioExistente.CorreoRecuperacionUsuario = dto.CorreoRecuperacion;

            // Si se envía una nueva contraseña, hashearla y actualizar
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuarioExistente.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                // Si se cambia la contraseña, podría quitarse la bandera de temporal
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
            if (usuarioExistente == null)
                return false;

            _context.Usuarios.Remove(usuarioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}