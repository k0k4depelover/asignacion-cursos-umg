using Asignacion.Web.Models.DTOs.Auth;
using Asignacion.Web.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Asignacion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión con correo y contraseña
        /// </summary>
        /// <param name="request">Credenciales de acceso</param>
        /// <returns>Token JWT + información del usuario</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log del error (se puede agregar ILogger)
                return StatusCode(500, new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        // Endpoint para refresh token (opcional, se implementará después)
        // [HttpPost("refresh")]
        // public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request) { ... }

        // Endpoint para logout (opcional)
        // [HttpPost("logout")]
        // public async Task<IActionResult> Logout() { ... }
    }
}