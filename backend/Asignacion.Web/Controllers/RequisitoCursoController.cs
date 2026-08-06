using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RequisitoCursoController : ControllerBase
    {
        private readonly IRequisitoCursoService _requisitoCursoService;
        public RequisitoCursoController(IRequisitoCursoService requisitoCursoService)
        {
            _requisitoCursoService = requisitoCursoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<RequisitoCurso>>> ObtenerTodosRequisitosCursosAsync()
        {
            var requisitosCursosDb = await _requisitoCursoService.ObtenerTodosRequisitosCursosAsync();
            return Ok(requisitosCursosDb);
        }

        [HttpGet("{idRequisito}")]
        public async Task<ActionResult<RequisitoCurso>> ObtenerRequisitoCursoPorIdAsync(int idRequisito)
        {
            var requisitoCursoDb = await _requisitoCursoService.ObtenerRequisitoCursoPorIdAsync(idRequisito);
            if (requisitoCursoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(requisitoCursoDb);
        }

        [HttpPost]
        public async Task<ActionResult<RequisitoCurso>> CrearRequisitoCursoAsync(RequisitoCurso requisitoCurso)
        {
            var requisitoCursoCreado = await _requisitoCursoService.CrearRequisitoCursoAsync(requisitoCurso);
            return CreatedAtAction(nameof(ObtenerRequisitoCursoPorIdAsync), new { idRequisito = requisitoCursoCreado.IdRequisitoCurso }, requisitoCursoCreado); // 201 CREATED
        }

        [HttpPut("{idRequisito}")]
        public async Task<IActionResult> ActualizarRequisitoCursoAsync(int idRequisito, RequisitoCurso requisitoCurso)
        {
            var requisitoCursoActualizado = await _requisitoCursoService.ActualizarRequisitoCursoAsync(idRequisito, requisitoCurso);
            if (!requisitoCursoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idRequisito}")]
        public async Task<IActionResult> EliminarRequisitoCursoAsync(int idRequisito)
        {
            var requisitoCursoEliminado = await _requisitoCursoService.EliminarRequisitoCursoAsync(idRequisito);
            if (!requisitoCursoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
