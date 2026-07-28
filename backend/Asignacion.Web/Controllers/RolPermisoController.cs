using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolPermisoController : ControllerBase
    {
        private readonly IRolPermisoService _rolPermisoService;
        public RolPermisoController(IRolPermisoService rolPermisoService)
        {
            _rolPermisoService = rolPermisoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RolPermiso>>> ObtenerTodosRolesPermisosAsync()
        {
            var rolesPermisosDb = await _rolPermisoService.ObtenerTodosRolesPermisosAsync();
            return Ok(rolesPermisosDb);
        }

        [HttpGet("{idRol}/{idPermiso}")]
        public async Task<ActionResult<RolPermiso>> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso)
        {
            var rolPermisoDb = await _rolPermisoService.ObtenerRolPermisoPorIdAsync(idRol, idPermiso);
            if (rolPermisoDb == null)
            {
                return NotFound();
            }
            return Ok(rolPermisoDb);
        }

        [HttpPost]
        public async Task<ActionResult<RolPermiso>> CrearRolPermisoAsync(RolPermiso rolPermiso)
        {
            var rolPermisoCreado = await _rolPermisoService.CrearRolPermisoAsync(rolPermiso);
            return CreatedAtAction(nameof(ObtenerRolPermisoPorIdAsync), new { idRol = rolPermisoCreado.IdRol, idPermiso = rolPermisoCreado.IdPermiso }, rolPermisoCreado);
        }

        [HttpDelete("{idRol}/{idPermiso}")]
        public async Task<IActionResult> EliminarRolPermisoAsync(int idRol, int idPermiso)
        {
            var rolPermisoEliminado = await _rolPermisoService.EliminarRolPermisoAsync(idRol, idPermiso);
            if (!rolPermisoEliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
