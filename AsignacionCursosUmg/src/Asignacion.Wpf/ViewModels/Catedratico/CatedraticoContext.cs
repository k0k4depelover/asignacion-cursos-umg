namespace Asignacion.Wpf.ViewModels.Catedratico;

/// <summary>
/// Shares "which sección is currently being drilled into" across the Catedrático shell's
/// three screens (Mis Secciones -> Estudiantes por Sección -> Calificaciones), since each
/// page ViewModel is resolved fresh from DI on navigation and can't take constructor params.
/// Registered as a DI singleton — safe because only one user session is active at a time.
/// </summary>
public class CatedraticoContext
{
    public int IdSeccionSeleccionada { get; set; }
    public string SeccionCodigo { get; set; } = "";
}
