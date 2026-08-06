using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Usuario;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

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
                return NotFound();

            return Ok(usuarioDb);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuarioAsync([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioCreado = await _usuarioService.CrearUsuarioAsync(dto);
                return CreatedAtAction(nameof(ObtenerUsuarioPorIdAsync), new { idUsuario = usuarioCreado.IdUsuario }, usuarioCreado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al crear el usuario." });
            }
        }

        [HttpPut("{idUsuario}")]
        public async Task<IActionResult> ActualizarUsuarioAsync(int idUsuario, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actualizado = await _usuarioService.ActualizarUsuarioAsync(idUsuario, dto);
                if (!actualizado)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error interno al actualizar el usuario." });
            }
        }

        [HttpDelete("{idUsuario}")]
        public async Task<IActionResult> EliminarUsuarioAsync(int idUsuario)
        {
            var eliminado = await _usuarioService.EliminarUsuarioAsync(idUsuario);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}