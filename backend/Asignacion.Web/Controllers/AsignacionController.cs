using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;
using AsignacionModel = Asignacion.Web.Models.Asignacion;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AsignacionController : ControllerBase
    {
        private readonly IAsignacionService _asignacionService;

        public AsignacionController(IAsignacionService asignacionService) {
            _asignacionService = asignacionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<AsignacionModel>>> ObtenerTodasAsignacionesAsync()
        {
            var asignacionesDb = await _asignacionService.ObtenerTodasAsignacionesAsync();
            return Ok(asignacionesDb);
        }

        [HttpGet("{idAsignacion}")]
        public async Task<ActionResult<AsignacionModel>> ObtenerAsignacionPorIdAsync(int idAsignacion)
        {
            var asignacionDb = await _asignacionService.ObtenerAsignacionPorIdAsync(int idAsignacion);
            if (asiignacionDb == null) {
                return NotFound(); // Codigo 404 
            }

            return Ok(asignacionDb);
        }

        [HttpPost]
        public async Task<ActionResult<AsignacionModel>> CrearAsignacionAsync(AsignacionModel asignacion)
        {
            var asignacionCreada = await _asignacionService.CrearAsignacionAsync(asignacion);

            return CreatedAtAction(nameof(ObtenerAsignacionPorIdAsync), new { idAsignacion = asignacionCreada.IdAsignacion }, asignacionCreada); // 201 CREATED
        }
        

        [HttpPut("{idAsignacion}")]
        public async Task<IActionResult> ActualizarAsignacionAsync(int idAsignacion, AsignacionModel asignacion)
        {
            var asignacionActualizada = await _asignacionService.ActualizarAsignacionAsync(idAsignacion, asignacion);

            if (!asignacionActualizada)
            {
                return NotFound();
            }

            return NoContent(); // 204 NO CONTENT 
        }


        [HttpDelete("{idAsignacion}")]
        public async Task<IActionResult> EliminarAsignacionAsync(int idAsignacion)
        {
            var asignacionEliminada = await _asignacionService.EliminarAsignacionAsync(idAsignacion);

            if (!asignacionEliminada)
            {
                return NotFound();
            }

            return NoContent(); // 204 NO CONTENT 
        }
    }
}