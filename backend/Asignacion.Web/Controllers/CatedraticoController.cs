using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CatedraticoController : ControllerBase
    {
        private readonly ICatedraticoService _catedraticoService;
        public CatedraticoController(ICatedraticoService catedraticoService)
        {
            _catedraticoService = catedraticoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Catedratico>>> ObtenerTodosCatedraticosAsync()
        {
            var catedraticosDb = await _catedraticoService.ObtenerTodosCatedraticosAsync();
            return Ok(catedraticosDb);
        }
        [HttpGet("{idCatedratico}")]
        public async Task<ActionResult<Catedratico>> ObtenerCatedraticoPorIdAsync(int idCatedratico)
        {
            var catedraticoDb = await _catedraticoService.ObtenerCatedraticoPorIdAsync(idCatedratico);
            if (catedraticoDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(catedraticoDb);
        }
        [HttpPost]
        public async Task<ActionResult<Catedratico>> CrearCatedraticoAsync(Catedratico catedratico)
        {
            var catedraticoCreado = await _catedraticoService.CrearCatedraticoAsync(catedratico);
            return CreatedAtAction(nameof(ObtenerCatedraticoPorIdAsync), new { idCatedratico = catedraticoCreado.IdCatedratico }, catedraticoCreado); // 201 CREATED
        }

        [HttpPut("{idCatedratico}")]
        public async Task<IActionResult> ActualizarCatedraticoAsync(int idCatedratico, Catedratico catedratico)
        {
            var catedraticoActualizado = await _catedraticoService.ActualizarCatedraticoAsync(idCatedratico, catedratico);
            if (!catedraticoActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idCatedratico}")]
        public async Task<IActionResult> EliminarCatedraticoAsync(int idCatedratico)
        {
            var catedraticoEliminado = await _catedraticoService.EliminarCatedraticoAsync(idCatedratico);
            if (!catedraticoEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}
