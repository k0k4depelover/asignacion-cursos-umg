using System.Collections.ObjectModel;
using System.Linq;
using Asignacion.Services.Matricula;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Estudiante;

public partial class MisCalificacionesViewModel : ObservableObject
{
    private readonly IDetalleAsignacionService _service;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private ObservableCollection<DetalleAsignacionDto> calificaciones = new();

    [ObservableProperty]
    private string? mensajeError;

    public MisCalificacionesViewModel(IDetalleAsignacionService service, CurrentSessionService session)
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
            var historial = await _service.GetHistorialByEstudianteAsync(idEstudiante.Value);
            Calificaciones = new ObservableCollection<DetalleAsignacionDto>(historial.Where(d => d.NotaFinal is not null));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
