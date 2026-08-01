using System.Collections.ObjectModel;
using Asignacion.Services.Matricula;
using Asignacion.Wpf.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels.Catedratico;

public partial class SeccionEstudiantesViewModel : ObservableObject
{
    private readonly IDetalleAsignacionService _service;
    private readonly CatedraticoContext _contexto;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private ObservableCollection<DetalleAsignacionDto> estudiantes = new();

    [ObservableProperty]
    private string? mensajeError;

    public string SeccionCodigo => _contexto.SeccionCodigo;

    public SeccionEstudiantesViewModel(IDetalleAsignacionService service, CatedraticoContext contexto, IMessenger messenger)
    {
        _service = service;
        _contexto = contexto;
        _messenger = messenger;
        _ = CargarAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        try
        {
            Estudiantes = new ObservableCollection<DetalleAsignacionDto>(await _service.GetRosterBySeccionAsync(_contexto.IdSeccionSeleccionada));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand]
    private void IrACalificar() => _messenger.Send(new NavegarACalificacionesMessage());
}
