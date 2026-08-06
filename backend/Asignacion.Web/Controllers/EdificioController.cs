using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EdificioController : ControllerBase
    {
        private readonly IEdificioService _edificioService;

        public EdificioController(IEdificioService edificioService)
        {
            _edificioService = edificioService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Edificio>>> ObtenerTodosEdificiosAsync()
        {
            var edificiosDb = await _edificioService.ObtenerTodosEdificiosAsync();
            return Ok(edificiosDb);
        }

        [HttpGet("{idEdificio}")]
        public async Task<ActionResult<Edificio>> ObtenerEdificioPorIdAsync(int idEdificio)
        {
            var edificioDb = await _edificioService.ObtenerEdificioPorIdAsync(idEdificio);
            if (edificioDb == null)
            {
                return NotFound(); // Código 404
            }

            return Ok(edificioDb);
        }

        [HttpPost]
        public async Task<ActionResult<Edificio>> CrearEdificioAsync(Edificio edificio)
        {
            var edificioCreado = await _edificioService.CrearEdificioAsync(edificio);

            return CreatedAtAction(nameof(ObtenerEdificioPorIdAsync), new { id = edificioCreado.IdEdificio }); // 201 CREATED
        }
        [HttpPut("{idEdificio}")]
        public async Task<IActionResult> ActualizarEdificioAsync(int idEdificio, Edificio edificio)
        {
            var edificioActualizado = await _edificioService.ActualizarEdificioAsync(idEdificio, edificio);

            if (!edificioActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idEdificio}")]
        public async Task<IActionResult> EliminarEdificioAsync(int idEdificio)
        {
            var edificioEliminado = await _edificioService.EliminarEdificioAsync(idEdificio);
            if (!edificioEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}