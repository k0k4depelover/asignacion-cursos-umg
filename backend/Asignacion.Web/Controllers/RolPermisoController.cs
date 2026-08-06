using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RolPermisoController : ControllerBase // Revisar Implementación y Controladores de Permiso y Rol Permiso
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

        [HttpGet("{idRolPermiso}")]
        public async Task<ActionResult<RolPermiso>> ObtenerRolPermisoPorIdAsync(int idRolPermiso)
        {
            var rolPermisoDb = await _rolPermisoService.ObtenerRolPermisoPorIdAsync(idRolPermiso);
            if (rolPermisoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(rolPermisoDb);
        }

        [HttpGet("rol/{idRol}")]
        public async Task<ActionResult<List<RolPermiso>>> ObtenerPorRolAsync(int idRol)
        {
            var permisosDelRol = await _rolPermisoService.ObtenerPorRolAsync(idRol);
            return Ok(permisosDelRol);
        }

        [HttpGet("permiso/{idPermiso}")]
        public async Task<ActionResult<List<RolPermiso>>> ObtenerPorPermisoAsync(int idPermiso)
        {
            var rolesDelPermiso = await _rolPermisoService.ObtenerPorPermisoAsync(idPermiso);
            return Ok(rolesDelPermiso);
        }

        [HttpPost]
        public async Task<ActionResult<RolPermiso>> CrearRolPermisoAsync(RolPermiso rolPermiso)
        {
            var rolPermisoCreado = await _rolPermisoService.CrearRolPermisoAsync(rolPermiso);
            return CreatedAtAction(nameof(ObtenerRolPermisoPorIdAsync), new { idRolPermiso = rolPermisoCreado.IdRolPermiso }, rolPermisoCreado); // 201 CREATED
        }

        [HttpPut("{idRolPermiso}")]
        public async Task<IActionResult> ActualizarRolPermisoAsync(int idRolPermiso, RolPermiso rolPermiso)
        {
            var rolPermisoActualizado = await _rolPermisoService.ActualizarRolPermisoAsync(idRolPermiso, rolPermiso);
            if (!rolPermisoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idRolPermiso}")]
        public async Task<IActionResult> EliminarRolPermisoAsync(int idRolPermiso)
        {
            var rolPermisoEliminado = await _rolPermisoService.EliminarRolPermisoAsync(idRolPermiso);
            if (!rolPermisoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
