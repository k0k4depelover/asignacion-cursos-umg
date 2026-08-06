using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class InscripcionController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;
        public InscripcionController(IInscripcionService inscripcionService)
        {
            _inscripcionService = inscripcionService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Inscripcion>>> ObtenerTodasInscripcionesAsync()
        {
            var inscripcionesDb = await _inscripcionService.ObtenerTodasInscripcionesAsync();
            return Ok(inscripcionesDb);
        }
        [HttpGet("{idInscripcion}")]
        public async Task<ActionResult<Inscripcion>> ObtenerInscripcionPorIdAsync(int idInscripcion)
        {
            var inscripcionDb = await _inscripcionService.ObtenerInscripcionPorIdAsync(idInscripcion);
            if (inscripcionDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(inscripcionDb);
        }
        [HttpPost]
        public async Task<ActionResult<Inscripcion>> CrearInscripcionAsync(Inscripcion inscripcion)
        {
            var inscripcionCreada = await _inscripcionService.CrearInscripcionAsync(inscripcion);
            return CreatedAtAction(nameof(ObtenerInscripcionPorIdAsync), new { idInscripcion = inscripcionCreada.IdInscripcion }, inscripcionCreada); // 201 CREATED
        }

        [HttpPut("{idInscripcion}")]
        public async Task<IActionResult> ActualizarInscripcionAsync(int idInscripcion, Inscripcion inscripcion)
        {
            var inscripcionActualizada = await _inscripcionService.ActualizarInscripcionAsync(idInscripcion, inscripcion);
            if (!inscripcionActualizada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idInscripcion}")]
        public async Task<IActionResult> EliminarInscripcionAsync(int idInscripcion)
        {
            var inscripcionEliminada = await _inscripcionService.EliminarInscripcionAsync(idInscripcion);
            if (!inscripcionEliminada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
