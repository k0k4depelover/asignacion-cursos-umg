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

        [HttpGet("{idHorario}")]
        public async Task<ActionResult<HorarioSeccion>> ObtenerHorarioSeccionPorIdAsync(int idHorario)
        {
            var horarioSeccionDb = await _horarioSeccionService.ObtenerHorarioSeccionPorIdAsync(idHorario);
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
            return CreatedAtAction(nameof(ObtenerHorarioSeccionPorIdAsync), new { idHorario = horarioSeccionCreado.IdHorario }, horarioSeccionCreado); // 201 CREATED
        }

        [HttpPut("{idHorario}")]
        public async Task<IActionResult> ActualizarHorarioSeccionAsync(int idHorario, HorarioSeccion horarioSeccion)
        {
            var horarioActualizado = await _horarioSeccionService.ActualizarHorarioSeccionAsync(idHorario, horarioSeccion);
            if (!horarioActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idHorario}")]
        public async Task<IActionResult> EliminarHorarioSeccionAsync(int idHorario)
        {
            var horarioSeccionEliminado = await _horarioSeccionService.EliminarHorarioSeccionAsync(idHorario);
            if (!horarioSeccionEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
