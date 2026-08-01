using System.ComponentModel.DataAnnotations;

namespace Asignacion.Web.Models.DTOs.Usuario
{

    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MaxLength(255)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo de login es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [MaxLength(255)]
        public string CorreoLogin { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? CorreoRecuperacion { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        public bool TienePassTemporal { get; set; } = false;

        [Required(ErrorMessage = "El estado del usuario es obligatorio.")]
        public string EstadoUsuario { get; set; } = "activo";

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int IdRol { get; set; }
    }
}