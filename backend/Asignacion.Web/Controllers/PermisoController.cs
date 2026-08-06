using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PermisoController : ControllerBase
    {
        private readonly IPermisoService _permisoService;
        public PermisoController(IPermisoService permisoService)
        {
            _permisoService = permisoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Permiso>>> ObtenerTodosPermisosAsync()
        {
            var permisosDb = await _permisoService.ObtenerTodosPermisosAsync();
            return Ok(permisosDb);
        }
        [HttpGet("{idPermiso}")]
        public async Task<ActionResult<Permiso>> ObtenerPermisoPorIdAsync(int idPermiso)
        {
            var permisoDb = await _permisoService.ObtenerPermisoPorIdAsync(idPermiso);
            if (permisoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(permisoDb);
        }

        [HttpPost]
        public async Task<ActionResult<Permiso>> CrearPermisoAsync(Permiso permiso)
        {
            var permisoCreado = await _permisoService.CrearPermisoAsync(permiso);
            return CreatedAtAction(nameof(ObtenerPermisoPorIdAsync), new { id = permisoCreado.IdPermiso }); // 201 CREATED
        }

        [HttpPut("{idPermiso}")]
        public async Task<IActionResult> ActualizarPermisoAsync(int idPermiso, Permiso permiso)
        {
            var permisoActualizado = await _permisoService.ActualizarPermisoAsync(idPermiso, permiso);
            if (!permisoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idPermiso}")]
        public async Task<IActionResult> EliminarPermisoAsync(int idPermiso)
        {
            var permisoEliminado = await _permisoService.EliminarPermisoAsync(idPermiso);
            if (!permisoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}