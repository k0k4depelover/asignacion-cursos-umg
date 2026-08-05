using System.Linq;
using Asignacion.Services.Matricula;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Estudiante;

public partial class MiPagoViewModel : ObservableObject
{
    private readonly IInscripcionService _inscripcionService;
    private readonly IAsignacionService _asignacionService;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private InscripcionDto? inscripcion;

    [ObservableProperty]
    private AsignacionDto? asignacion;

    [ObservableProperty]
    private string? mensajeError;

    public MiPagoViewModel(IInscripcionService inscripcionService, IAsignacionService asignacionService, CurrentSessionService session)
    {
        _inscripcionService = inscripcionService;
        _asignacionService = asignacionService;
        _session = session;
        _ = CargarAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        var idEstudiante = _session.Current?.IdEstudiante;
        if (idEstudiante is null)
        {
            MensajeError = "No hay un perfil de estudiante asociado a esta cuenta.";
            return;
        }

        try
        {
            var inscripciones = await _inscripcionService.GetByEstudianteAsync(idEstudiante.Value);
            Inscripcion = inscripciones.FirstOrDefault();
            if (Inscripcion is null)
            {
                MensajeError = "No tiene inscripciones registradas.";
                return;
            }

            Asignacion = await _asignacionService.GetByInscripcionAsync(Inscripcion.Id);
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
