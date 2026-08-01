using System.ComponentModel.DataAnnotations;

namespace Asignacion.Web.Models.DTOs.Usuario
{
    public class UpdateUserDto
    {
        [MaxLength(255)]
        public string? NombreUsuario { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [MaxLength(255)]
        public string? CorreoLogin { get; set; }

        [MaxLength(255)]
        public string? CorreoRecuperacion { get; set; }

        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(100)]
        public string? Password { get; set; } // Opcional: si se envía, se actualiza

        public bool? TienePassTemporal { get; set; }

        public string? EstadoUsuario { get; set; }

        public int? IdRol { get; set; }
    }
}