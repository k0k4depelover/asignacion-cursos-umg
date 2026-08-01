using System.Collections.ObjectModel;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels.Catedratico;

public partial class MisSeccionesViewModel : ObservableObject
{
    private readonly ISeccionService _service;
    private readonly CurrentSessionService _session;
    private readonly CatedraticoContext _contexto;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private ObservableCollection<SeccionDto> secciones = new();

    [ObservableProperty]
    private SeccionDto? seleccionada;

    [ObservableProperty]
    private string? mensajeError;

    public MisSeccionesViewModel(ISeccionService service, CurrentSessionService session, CatedraticoContext contexto, IMessenger messenger)
    {
        _service = service;
        _session = session;
        _contexto = contexto;
        _messenger = messenger;
        _ = CargarAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        var idCatedratico = _session.Current?.IdCatedratico;
        if (idCatedratico is null)
        {
            MensajeError = "No hay un perfil de catedrático asociado a esta cuenta.";
            return;
        }

        try
        {
            Secciones = new ObservableCollection<SeccionDto>(await _service.GetByCatedraticoAsync(idCatedratico.Value));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(HaySeleccion))]
    private void VerEstudiantes()
    {
        if (Seleccionada is null)
        {
            return;
        }

        _contexto.IdSeccionSeleccionada = Seleccionada.Id;
        _contexto.SeccionCodigo = Seleccionada.Codigo;
        _messenger.Send(new NavegarASeccionEstudiantesMessage());
    }

    private bool HaySeleccion() => Seleccionada is not null;

    partial void OnSeleccionadaChanged(SeccionDto? value) => VerEstudiantesCommand.NotifyCanExecuteChanged();
}
