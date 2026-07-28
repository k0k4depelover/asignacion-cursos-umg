using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DetalleAsignacionController : ControllerBase
    {
        private readonly IDetalleAsignacionService detalleAsignacionService;
        public DetalleAsignacionController(IDetalleAsignacionService detalleAsignacionService)
        {
            _detalleAsignacionService = detalleAsignacionService;
        }
        [HttpGet]
        public async Task<ActionResult<List<DetalleAsignacion>>> ObtenerTodosDetallesAsignacionesAsync()
        {
            var detalleAsignacionDb = await _detalleAsignacionService.ObtenerTodosDetallesAsignacionesAsync();
            return Ok(detalleAsignacionDb);
        }
        [HttpGet("{idDetalleAsignacion}")]
        public async Task<ActionResult<DetalleAsignacion>> ObtenerDetalleAsignacionPorIdAsync(int idDetalleAsignacion)
        {
            var detalleAsignacionDb = await _detalleAsignacionService.ObtenerDetalleAsignacionPorIdAsync(idDetalleAsignacion);
            if (detalleAsignacionDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(detalleAsignacionDb);
        }
        [HttpPost]
        public async Task<ActionResult<DetalleAsignacion>> CrearDetalleAsync(DetalleAsignacion detalleAsignacion)
        {
            var detalleAsignacionCreado = await _detalleAsignacionService.CrearDetalleAsignacionAsync(detalleAsignacion);
            return CreatedAtAction(nameof(ObtenerDetalleAsignacionPorIdAsync), new { id = detalleAsignacionCreado.IdDetalleAsignacion }); // 201 CREATED
        }
        [HttpPut("{idDetalleAsignacion}")]
        public async Task<IActionResult> ActualizarDetalleAsignacionAsync(int idDetalleAsignacion, DetalleAsignacion detalleAsignacion)
        {
            var detalleAsignacionActualizado = await _detalleAsignacionService.ActualizarDetalleAsignacionAsync(idDetalleAsignacion, detalleAsignacion);
            if (!detalleAsignacionActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idDetalleAsignacion}")]
        public async Task<IActionResult> EliminarDetalleAsignacionAsync(int idDetalleAsignacion)
        {
            var detalleAsignacionEliminado = await _detalleAsignacionService.EliminarDetalleAsignacionAsync(idDetalleAsignacion);
            if (!detalleAsignacionEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}