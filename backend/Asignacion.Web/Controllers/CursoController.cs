using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CursoController : ControllerBase
    {
        private readonly ICursoService _cursoService;
        public CursoController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> ObtenerTodosCursosAsync()
        {
            var cursosDb = await _cursoService.ObtenerTodosCursosAsync();
            return Ok(cursosDb);
        }
        [HttpGet("{idCurso}")]
        public async Task<ActionResult<Curso>> ObtenerCursoPorIdAsync(int idCurso)
        {
            var cursoDb = await _cursoService.ObtenerCursoPorIdAsync(idCurso);
            if (cursoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(cursoDb);
        }
        [HttpPost]
        public async Task<ActionResult<Curso>> CrearCursoAsync(Curso curso)
        {
            var cursoCreado = await _cursoService.CrearCursoAsync(curso);

            return CreatedAtAction(nameof(ObtenerCursoPorIdAsync), new { id = cursoCreado.IdCurso }); // 201 CREATED
        }
        [HttpPut("{idCurso}")]
        public async Task<IActionResult> ActualizarCursoAsync(int idCurso, Curso curso)
        {
            var cursoActualizado = await _cursoService.ActualizarCursoAsync(idCurso, curso);
            if (!cursoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idCurso}")]
        public async Task<IActionResult> EliminarCursoAsync(int idCurso)
        {
            var cursoEliminado = await _cursoService.EliminarCursoAsync(idCurso);
            if (!cursoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}