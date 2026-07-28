using Microsoft.AspNetCore.Mvc;
using Asignacion.Web.Models;
using Asignacion.Web.Services;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SalonController : ControllerBase
    {
        private readonly ISalonService _salonService;
        public SalonController(ISalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Salon>>> ObtenerTodosSalonesAsync()
        {
            var salonesDb = await _salonService.ObtenerTodosSalonesAsync();
            return Ok(salonesDb);
        }

        [HttpGet("{idSalon}")]
        public async Task<ActionResult<Salon>> ObtenerSalonPorIdAsync(int idSalon)
        {
            var salonDb = await _salonService.ObtenerSalonPorIdAsync(idSalon);
            if (salonDb == null)
            {
                return NotFound(); // Código 404
            }
            return Ok(salonDb);
        }

        [HttpPost]
        public async Task<ActionResult<Salon>> CrearSalonAsync(Salon salon)
        {
            var salonCreado = await _salonService.CrearSalonAsync(salon);
            return CreatedAtAction(nameof(ObtenerSalonPorIdAsync), new { idSalon = salonCreado.IdSalon }); // 201 CREATED
        }

        [HttpPut("{idSalon}")]
        public async Task<IActionResult> ActualizarSalonAsync(int idSalon, Salon salon)
        {
            var salonActualizado = await _salonService.ActualizarSalonAsync(idSalon, salon);
            if (!salonActualizado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }

        [HttpDelete("{idSalon}")]
        public async Task<IActionResult> EliminarSalonAsync(int idSalon)
        {
            var salonEliminado = await _salonService.EliminarSalonAsync(idSalon);
            if (!salonEliminado)
            {
                return NotFound(); // Código 404
            }
            return NoContent(); // 204 NO CONTENT
        }
    }
}