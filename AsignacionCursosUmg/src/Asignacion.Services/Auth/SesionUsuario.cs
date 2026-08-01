namespace Asignacion.Services.Auth;

public record SesionUsuario(
    int IdUsuario,
    string NombreUsuario,
    string CorreoLogin,
    string RolNombre,
    int? IdEstudiante,
    int? IdCatedratico);
