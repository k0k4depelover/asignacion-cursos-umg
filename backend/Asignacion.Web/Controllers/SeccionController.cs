using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SeccionController : ControllerBase
    {
        private readonly ISeccionService _seccionService;
        public SeccionController(ISeccionService seccionService)
        {
            _seccionService = seccionService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Seccion>>> ObtenerTodasSeccionesAsync()
        {
            var seccionesDb = await _seccionService.ObtenerTodasSeccionesAsync();
            return Ok(seccionesDb);
        }
        [HttpGet("{idSeccion}")]
        public async Task<ActionResult<Seccion>> ObtenerSeccionPorIdAsync(int idSeccion)
        {
            var seccionDb = await _seccionService.ObtenerSeccionPorIdAsync(idSeccion);
            if (seccionDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(seccionDb);
        }
        [HttpPost]
        public async Task<ActionResult<Seccion>> CrearSeccionAsync(Seccion seccion)
        {
            var seccionCreada = await _seccionService.CrearSeccionAsync(seccion);
            return CreatedAtAction(nameof(ObtenerSeccionPorIdAsync), new { idSeccion = seccionCreada.IdSeccion }); // 201 CREATED
        }

        [HttpPut("{idSeccion}")]
        public async Task<IActionResult> ActualizarSeccionAsync(int idSeccion, Seccion seccion)
        {
            var seccionActualizada = await _seccionService.ActualizarSeccionAsync(idSeccion, seccion);
            if (!seccionActualizada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idSeccion}")]
        public async Task<IActionResult> EliminarSeccionAsync(int idSeccion)
        {
            var seccionEliminada = await _seccionService.EliminarSeccionAsync(idSeccion);
            if (!seccionEliminada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

    }
}