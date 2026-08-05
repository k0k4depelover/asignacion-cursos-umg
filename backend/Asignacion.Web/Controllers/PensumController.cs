using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PensumController : ControllerBase
    {
        private readonly IPensumService pensumService;
        public PensumController(IPensumService pensumService)
        {
            _pensumService = pensumService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Pensum>>> ObtenerTodosPensumsAsync()
        {
            var pensumsDb = await _pensumService.ObtenerTodosPensumsAsync();
            return Ok(pensumsDb);
        }
        [HttpGet("{idPensum}")]
        public async Task<ActionResult<Pensum>> ObtenerPensumPorIdAsync(int idPensum)
        {
            var pensumDb = await _pensumService.ObtenerPensumPorIdAsync(int idPensum);
            if (pensumDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(pensumDb);
        }
        [HttpPost]
        public async Task<ActionResult<Pensum>> CrearPensumAsync(Pensum pensum)
        {
            var pensumCreado = await _pensumService.CrearPensumAsync(Pensum pensum);
            return CreatedAtAction(nameof(ObtenerPensumPorIdAsync), new { id = pensumCreado.IdPensum }); // 201 CREATED
        }
        [HttpPut("{idPensum}")]
        public async Task<IActionResult> ActualizarPensumAsync(int idPensum, Pensum pensum)
        {
            var pensumActualizado = await _pensumService.ActualizarPensumAsync(int idPensum, Pensum pensum);
            if (!pensumActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idPensum}")]
        public async Task<IActionResult> EliminarPensumAsync(int idPensum)
        {
            var pensumEliminado = await _pensumService.EliminarPensumAsync(idPensum);
            if (!pensumEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}