using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;
        public EstudianteController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Estudiante>>> ObtenerTodosEstudiantesAsync()
        {
            var estudiantesDb = await _estudianteService.ObtenerTodosEstudiantesAsync();
            return Ok(estudiantesDb);
        }
        [HttpGet("{idEstudiante}")]
        public async Task<ActionResult<Estudiante>> ObtenerEstudiantePorIdAsync(int idEstudiante)
        {
            var estudianteDb = await _estudianteService.ObtenerEstudiantePorIdAsync(idEstudiante);
            if (estudianteDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(estudianteDb);
        }
        [HttpPost]
        public async Task<ActionResult<Estudiante>> CrearEstudianteAsync(Estudiante estudiante)
        {
            var estudianteCreado = await _estudianteService.CrearEstudianteAsync(estudiante);
            return CreatedAtAction(nameof(ObtenerEstudiantePorIdAsync), new { idEstudiante = estudianteCreado.IdEstudiante }); // 201 CREATED
        }
        [HttpPut("{idEstudiante}")]
        public async Task<IActionResult> ActualizarEstudianteAsync(int idEstudiante, Estudiante estudiante)
        {
            var estudianteActualizado = await _estudianteService.ActualizarEstudianteAsync(idEstudiante, estudiante);
            if (!estudianteActualizado)
            {
                return NotFound(); // Código 404
            }

            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idEstudiante}")]
        public async Task<IActionResult> EliminarEstudianteAsync(int idEstudiante)
        {
            var estudianteEliminado = await _estudianteService.EliminarEstudianteAsync(idEstudiante);
            if (!estudianteEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}