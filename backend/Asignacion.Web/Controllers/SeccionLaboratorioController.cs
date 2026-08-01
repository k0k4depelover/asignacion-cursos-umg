using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SeccionLaboratorioController : ControllerBase
    {
        private readonly ISeccionLaboratorioService seccionLaboratorioService;
        public SeccionLaboratorioController(ISeccionLaboratorioService seccionLaboratorioService)
        {
            _seccionLaboratorioService = seccionLaboratorioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SeccionLaboratorio>>> ObtenerTodasSeccionesLaboratoriosAsync()
        {
            var seccionLaboratorioDb = await _seccionLaboratorioService.ObtenerTodasSeccionesLaboratoriosAsync();
            return Ok(seccionLaboratorioDb);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeccionLaboratorio>> ObtenerSeccionLaboratorioPorIdAsync(int idSeccionLaboratorio)
        {
            var seccionLaboratorioDb = await _seccionLaboratorioService.ObtenerSeccionLaboratorioPorIdAsync(idSeccionLaboratorio);
            if (seccionLaboratorioDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(seccionDb);
        }

        [HttpPost]
        public async Task<ActionResult<SeccionLaboratorio>> CrearSeccionLaboratorioAsync(SeccionLaboratorio seccionLaboratorio)
        {
            var seccionLaboratorioCreada = await _seccionLaboratorioService.CrearSeccionLaboratorioAsync(seccionLaboratorio);
            return CreatedAtAction(nameof(ObtenerSeccionLaboratorioPorIdAsync), new { id = seccionLaboratorioCreada.Id }); // 201 CREATED
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSeccionLaboratorioAsync(int idSeccionLaboratorio, SeccionLaboratorio seccionLaboratorio)
        {
            var seccionLaboratorioActualizada = await _seccionLaboratorioService.ActualizarSeccionLaboratorioAsync(int idSeccionLaboratorio,SeccionLaboratorio seccionLaboratorio);
            if (!seccionLaboratorioActualizada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarSeccionLaboratorioAsync(int idSeccionLaboratorio)
        {
            var seccionLaboratorioEliminada = await _seccionLaboratorioService.EliminarSeccionLaboratorioAsync(idSeccionLaboratorio);
            if (!seccionLaboratorioEliminada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}