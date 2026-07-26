using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppContext _context


            public UsuarioService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosUsuariosAsync()
        {
            return await _context.Usuario.ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _context.Usuario.FindAsync(idUsuario);
        }

        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            _context.Usuario.Addsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ActualizarUsuarioAsync(int idUsuario, Usuario usuario)
        {
            var usuarioExistente = await _context.Usuario.FindAsync(idUsuario);
            if (usuarioExistente == null)
            {
                return false;
            }
            usuarioExistente.IdUsuario = usuario.IdUsuario;
            usuarioExistente.NombreUsuario = usuario.NombreUsuario;
            usuarioExistente.CorreoLoginUsuario = usuario.CorreoLoginUsuario;
            usuarioExistente.CorreoRecuperacionUsuario = usuario.CorreoRecuperacionUsuario;
            usuarioExistente.ContrasenaHash = usuario.ContrasenaHash;
            usuarioExistente.TienePassTemporal = usuario.TienePassTemporal;
            usuarioExistente.EstadoUsuario = usuario.EstadoUsuario;
            usuarioExistente.FechaRegistroUsuario = usuario.FechaRegistroUsuario;
            usuarioExistente.IdRol = usuario.IdRol;
            return true;
        }

        public async Task<bool> EliminarUsuarioAsync(int idUsuario)
        {
            var usuarioExistente = _context.Usuario.FindAsync(idUsuario);
            if (usuarioExistente == null)
            {
                return false;
            }
            _context.Usuario.Remove(usuarioExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
