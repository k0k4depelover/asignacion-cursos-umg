using System.ComponentModel.DataAnnotations;

namespace Asignacion.Web.Models.DTOs.Auth
{

    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [MaxLength(255, ErrorMessage = "El correo no puede superar los 255 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(255, ErrorMessage = "La contraseña no puede superar los 255 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}