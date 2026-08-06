using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FacultadController : ControllerBase
    {
        private readonly IFacultadService _facultadService;
        public FacultadController(IFacultadService facultadService)
        {
            _facultadService = facultadService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Facultad>>> ObtenerTodasFacultadesAsync()
        {
            var facultadesDb = await _facultadService.ObtenerTodasFacultadesAsync();
            return Ok(facultadesDb);
        }
        [HttpGet("{idFacultad}")]
        public async Task<ActionResult<Facultad>> ObtenerFacultadPorIdAsync(int idFacultad)
        {
            var facultadDb = await _facultadService.ObtenerFacultadPorIdAsync(idFacultad);
            if (facultadDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(facultadDb);
        }
        [HttpPost]
        public async Task<ActionResult<Facultad>> CrearFacultadAsync(Facultad facultad)
        {
            var facultadCreada = await _facultadService.CrearFacultadAsync(facultad);
            return CreatedAtAction(nameof(ObtenerFacultadPorIdAsync), new { id = facultadCreada.IdFacultad }); // 201 CREATED
        }
        [HttpPut("{idFacultad}")]
        public async Task<IActionResult> ActualizarFacultadAsync(int idFacultad, Facultad facultad)
        {
            var facultadActualizada = await _facultadService.ActualizarFacultadAsync(idFacultad, facultad);
            if (!facultadActualizada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
        [HttpDelete("{idFacultad}")]
        public async Task<IActionResult> EliminarFacultadAsync(int idFacultad)
        {
            var facultadEliminada = await _facultadService.EliminarFacultadAsync(idFacultad);
            if (!facultadEliminada)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}