using System.Collections.ObjectModel;
using Asignacion.Services.Matricula;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Estudiante;

public partial class MisCursosViewModel : ObservableObject
{
    private readonly IDetalleAsignacionService _service;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private ObservableCollection<DetalleAsignacionDto> cursos = new();

    [ObservableProperty]
    private string? mensajeError;

    public MisCursosViewModel(IDetalleAsignacionService service, CurrentSessionService session)
    {
        _service = service;
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
            Cursos = new ObservableCollection<DetalleAsignacionDto>(await _service.GetHistorialByEstudianteAsync(idEstudiante.Value));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
