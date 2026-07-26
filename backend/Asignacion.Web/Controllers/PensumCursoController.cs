using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PensumCursoController : ControllerBase
    {
        private readonly IPensumCursoService pensumCursoService;
        public PensumCursoController(IPensumCursoService pensumCursoService)
        {
            _pensumCursoService = pensumCursoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<PensumCurso>>> ObtenerTodosPensumCursosAsync()
        {
            var pensumCursosDb = await _pensumCursoService.ObtenerTodosPensumCursosAsync();
            return Ok(pensumCursosDb);
        }
        [HttpGet("{idPensumCurso}")]
        public async Task<ActionResult<PensumCurso>> ObtenerPensumCursoPorIdAsync(int idPensumCurso)
        {
            var pensumCursoDb = await _pensumCursoService.ObtenerPensumCursoPorIdAsync(int idPensumCurso);
            if (pensumCursoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(pensumCursoDb);
        }
        [HttpPost]
        public async Task<ActionResult<PensumCurso>> CrearPensumCursoAsync(PensumCurso pensumCurso)
        {
            var pensumCursoCreado = await _pensumCursoService.CrearPensumCursoAsync(PensumCurso pensumCurso);
            return CreatedAtAction(nameof(ObtenerPensumCursoPorIdAsync), new { id = pensumCursoCreado.IdPensumCurso }); // 201 CREATED
        }
        [HttpPut("{idPensumCurso}")]
        public async Task<IActionResult> ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso)
        {
            var pensumCursoActualizado = await _pensumCursoService.ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso);
            if (!pensumCursoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idPensumCurso}")]
        public async Task<IActionResult> EliminarPensumCursoAsync(int idPensumCurso)
        {
            var pensumCursoEliminado = await _pensumCursoService.EliminarPensumCursoAsync(idPensumCurso);
            if (!pensumCursoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}