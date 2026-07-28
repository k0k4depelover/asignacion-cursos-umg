using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class HorarioSeccionController : ControllerBase
    {
        private readonly IHorarioSeccionService _horarioSeccionService;
        public HorarioSeccionController(IHorarioSeccionService horarioSeccionService)
        {
            _horarioSeccionService = horarioSeccionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<HorarioSeccion>>> ObtenerTodosHorariosSeccionesAsync()
        {
            var horarioSeccionDb = await _horarioSeccionService.ObtenerTodosHorariosSeccionesAsync();
            return Ok(horarioSeccionDb);
        }

        [HttpGet("{idHorarioSeccion}")]
        public async Task<ActionResult<HorarioSeccion>> ObtenerHorarioSeccionPorIdAsync(int idHorarioSeccion)
        {
            var horarioSeccionDb = await _horarioSeccionService.ObtenerHorarioSeccionPorIdAsync(idHorarioSeccion);
            if (horarioSeccionDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(horarioSeccionDb);
        }

        [HttpPost]
        public async Task<ActionResult<HorarioSeccion>> CrearHorarioSeccionAsync(HorarioSeccion horarioSeccion)
        {
            var horarioSeccionCreado = await _horarioSeccionService.CrearHorarioSeccionAsync(horarioSeccion);
            return CreatedAtAction(nameof(ObtenerHorarioSeccionPorIdAsync), new { idHorarioSeccion = horarioSeccionCreado.IdHorario }, horarioSeccionCreado); // 201 CREATED
        }

        [HttpPut("{idHorarioSeccion}")]
        public async Task<IActionResult> ActualizarHorarioSeccionAsync(int idHorarioSeccion, HorarioSeccion horarioSeccion)
        {
            var horarioActualizado = await _horarioSeccionService.ActualizarHorarioSeccionAsync(idHorarioSeccion, horarioSeccion);
            if (!horarioActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idHorarioSeccion}")]
        public async Task<IActionResult> EliminarHorarioSeccionAsync(int idHorarioSeccion)
        {
            var horarioSeccionEliminado = await _horarioSeccionService.EliminarHorarioSeccionAsync(idHorarioSeccion);
            if (!horarioSeccionEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}