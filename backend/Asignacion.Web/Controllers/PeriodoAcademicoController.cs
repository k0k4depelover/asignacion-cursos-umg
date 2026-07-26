using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PeriodoAcademicoController : ControllerBase
    {
        private readonly IPeriodoAcademicoService periodoAcademicoService;
        public PeriodoAcademicoController(IPeriodoAcademicoService periodoAcademicoService)
        {
            _periodoAcademicoService = periodoAcademicoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeriodoAcademico>>> ObtenerTodosPeriodosAcademicosAsync()
        {
            var periodoAcademicoDb = await _periodoAcademicoService.ObtenerTodosPeriodosAcademicosAsync();
            return Ok(periodoAcademicoDb);
        }

        [HttpGet("{idPeriodo}")]
        public async Task<ActionResult<PeriodoAcademico>> ObtenerPeriodoAcademicoPorIdAsync(int idPeriodoAcademico)
        {
            var periodoAcademicoDb = await _periodoAcademicoService.ObtenerPeriodoAcademicoPorIdAsync(int idPeriodoAcademico);
            if (periodoAcademicoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(periodoDb);
        }

        [HttpPost]
        public async Task<ActionResult<PeriodoAcademico>> CrearPeriodoAcademicoAsync(PeriodoAcademico periodoAcademico)
        {
            var periodoAcademicoCreado = await _periodoAcademicoService.CrearPeriodoAcademicoAsync(PeriodoAcademico periodoAcademico);
            return CreatedAtAction(nameof(ObtenerPeriodoAcademicoPorIdAsync), new { id = periodoCreado.IdPeriodoAcademico }); // 201 CREATED
        }

        [HttpPut("{idPeriodo}")]
        public async Task<IActionResult> ActualizarPeriodoAcademicoAsync(int idPeriodo, PeriodoAcademico periodoAcademico)
        {
            var periodoAcademicoActualizado = await _periodoAcademicoService.ActualizarPeriodoAsync(int idPeriodoAcademico, PeriodoAcademico periodoAcademico);
            if (!periodoAcademicoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idPeriodoAcademico}")]
        public async Task<IActionResult> EliminarPeriodoAcademicoAsync(int idPeriodoAcademico)
        {
            var periodoAcademicoEliminado = await _periodoAcademicoService.EliminarPeriodoAcademicoAsync(idPeriodoAcademico);
            if (!periodoAcademicoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}