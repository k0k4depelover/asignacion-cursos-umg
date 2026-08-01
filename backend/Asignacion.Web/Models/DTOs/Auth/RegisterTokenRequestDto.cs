using System.ComponentModel.DataAnnotations;

namespace Asignacion.Web.Models.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public int IdRol { get; set; } = 2; // Estudiante por defecto
    }
}