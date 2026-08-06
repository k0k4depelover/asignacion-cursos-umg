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

        [HttpGet("{idRol}/{idPermiso}")]
        public async Task<ActionResult<RolPermiso>> ObtenerRolPermisoPorIdAsync(int idRol, int idPermiso)
        {
            var rolPermisoDb = await _rolPermisoService.ObtenerRolPermisoPorIdAsync(idRol, idPermiso);
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
            return CreatedAtAction(nameof(ObtenerRolPermisoPorIdAsync), new { idRol = rolPermisoCreado.IdRol, idPermiso = rolPermisoCreado.IdPermiso }, rolPermisoCreado); // 201 CREATED
        }

        [HttpDelete("{idRol}/{idPermiso}")]
        public async Task<IActionResult> EliminarRolPermisoAsync(int idRol, int idPermiso)
        {
            var rolPermisoEliminado = await _rolPermisoService.EliminarRolPermisoAsync(idRol, idPermiso);
            if (!rolPermisoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
