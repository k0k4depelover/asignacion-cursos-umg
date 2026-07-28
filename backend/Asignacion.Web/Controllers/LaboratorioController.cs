using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LaboratorioController : ControllerBase
    {
        private readonly ILaboratorioService _laboratorioService;
        public LaboratorioController(ILaboratorioService laboratorioService)
        {
            _laboratorioService = laboratorioService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Laboratorio>>> ObtenerTodosLaboratoriosAsync()
        {
            var laboratoriosDb = await _laboratorioService.ObtenerTodosLaboratoriosAsync();
            return Ok(laboratoriosDb);
        }
        [HttpGet("{idLaboratorio}")]
        public async Task<ActionResult<Laboratorio>> ObtenerLaboratorioPorIdAsync(int idLaboratorio)
        {
            var laboratorioDb = await _laboratorioService.ObtenerLaboratorioPorIdAsync(idLaboratorio);
            if (laboratorioDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(laboratorioDb);
        }
        [HttpPost]
        public async Task<ActionResult<Laboratorio>> CrearLaboratorioAsync(Laboratorio laboratorio)
        {
            var laboratorioCreado = await _laboratorioService.CrearLaboratorioAsync(laboratorio);
            return CreatedAtAction(nameof(ObtenerLaboratorioPorIdAsync), new { idLaboratorio = laboratorioCreado.IdLaboratorio }); // 201 CREATED
        }
        [HttpPut("{idLaboratorio}")]
        public async Task<IActionResult> ActualizarLaboratorioAsync(int idLaboratorio, Laboratorio laboratorio)
        {
            var laboratorioActualizado = await _laboratorioService.ActualizarLaboratorioAsync(idLaboratorio, laboratorio);
            if (!laboratorioActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idLaboratorio}")]
        public async Task<IActionResult> EliminarLaboratorioAsync(int idLaboratorio)
        {
            var laboratorioEliminado = await _laboratorioService.EliminarLaboratorioAsync(idLaboratorio);
            if (!laboratorioEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}