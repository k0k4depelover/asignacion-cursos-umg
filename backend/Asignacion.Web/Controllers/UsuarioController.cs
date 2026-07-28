using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> ObtenerTodosUsuariosAsync()
        {
            var usuariosDb = await _usuarioService.ObtenerTodosUsuariosAsync();
            return Ok(usuariosDb);
        }

        [HttpGet("{idUsuario}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            var usuarioDb = await _usuarioService.ObtenerUsuarioPorIdAsync(idUsuario);
            if (usuarioDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(usuarioDb);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuarioAsync(Usuario usuario)
        {
            var usuarioCreado = await _usuarioService.CrearUsuarioAsync(Usuario usuario);
            return CreatedAtAction(nameof(ObtenerUsuarioPorIdAsync), new { id = usuarioCreado.IdUsuario }); // 201 CREATED
        }

        [HttpPut("{idUsuario}")]
        public async Task<IActionResult> ActualizarUsuarioAsync(int idUsuario, Usuario usuario)
        {
            var usuarioActualizado = await _usuarioService.ActualizarUsuarioAsync(idUsuario, usuario);
            if (!usuarioActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idUsuario}")]
        public async Task<IActionResult> EliminarUsuarioAsync(int idUsuario)
        {
            var usuarioEliminado = await _usuarioService.EliminarUsuarioAsync(idUsuario);
            if (!usuarioEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}