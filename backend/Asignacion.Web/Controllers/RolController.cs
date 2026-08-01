using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RolController : ControllerBase
    {
        private readonly IRolService rolService;
        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Rol>>> ObtenerTodosRolesAsync()
        {
            var rolesDb = await _rolService.ObtenerTodosRolesAsync();
            return Ok(rolesDb);
        }

        [HttpGet("{idRol}")]
        public async Task<ActionResult<Rol>> ObtenerRolPorIdAsync(int idRol)
        {
            var rolDb = await _rolService.ObtenerRolPorIdAsync(int idRol);
            if (rolDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(rolDb);
        }

        [HttpPost]
        public async Task<ActionResult<Rol>> CrearRolAsync(Rol rol)
        {
            var rolCreado = await _rolService.CrearRolAsync(Rol rol);
            return CreatedAtAction(nameof(ObtenerRolPorIdAsync), new { id = rolCreado.IdRol }); // 201 CREATED
        }

        [HttpPut("{idRol}")]
        public async Task<IActionResult> ActualizarRolAsync(int idRol, Rol rol)
        {
            var rolActualizado = await _rolService.ActualizarRolAsync(int idRol, Rol rol);
            if (!rolActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idRol}")]
        public async Task<IActionResult> EliminarRolAsync(int idRol)
        {
            var rolEliminado = await _rolService.EliminarRolAsync(idRol);
            if (!rolEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}