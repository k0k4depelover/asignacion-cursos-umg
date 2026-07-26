using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CarreraController : ControllerBase
    {
        private readonly ICarreraService carreraService;

        public CarreraController(ICarreraService carreraService)
        {
            _carreraService = carreraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Carrera>>> ObtenerTodasCarrerasAsync()
        {
            var carrerasDb = await _carreraService.ObtenerTodasCarrerasAsync();
            return Ok(carrerasDb);
        }

        [HttpGet("{idCarrera}")]
        public async Task<ActionResult<Carrera>> ObtenerCarreraPorIdAsync(int idCarrera)
        {
            var carreraDb = await _carreraService.ObtenerCarreraPorIdAsync(idCarrera);
            if (carreraDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(carreraDb);
        }

        [HttpPost]
        public async Task<ActionResult<Carrera>> CrearCarreraAsync(Carrera carrera)
        {
            var carreraCreada = await _carreraService.CrearCarreraAsync(Carrera carrera);

            return CreatedAtAction(nameof(ObtenerCarreraPorIdAsync), new { id = carreraCreada.IdCarrera }, carreraCreada); // 201 CREATED
        }
        [HttpPut("{idCarrera}")]
        public async Task<IActionResult> ActualizarCarreraAsync(int idCarrera, Carrera carrera)
        {
            var carreraActualizada = await _carreraService.ActualizarCarreraAsync(int idCarrera, Carrera carrera);

            if (!carreraActualizada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idCarrera}")]
        public async Task<IActionResult> EliminarCarreraAsync(int idCarrera)
        {
            var carreraEliminada = await _carreraService.EliminarCarreraAsync(idCarrera);
            if (!carreraEliminada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}