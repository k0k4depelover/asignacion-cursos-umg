using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Services.Personas;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class SeccionListViewModel : ObservableObject
{
    private readonly ISeccionService _seccionService;
    private readonly ICursoService _cursoService;
    private readonly IPeriodoAcademicoService _periodoService;
    private readonly ICatedraticoService _catedraticoService;
    private readonly ISalonService _salonService;
    private readonly ILaboratorioService _laboratorioService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<SeccionDto> secciones = new();

    [ObservableProperty]
    private SeccionDto? seccionSeleccionada;

    [ObservableProperty]
    private ObservableCollection<HorarioSeccionDto> horarios = new();

    [ObservableProperty]
    private ObservableCollection<SeccionLaboratorioDto> laboratoriosSeccion = new();

    [ObservableProperty]
    private ObservableCollection<LaboratorioDto> laboratoriosDisponibles = new();

    [ObservableProperty]
    private string diaSemanaNuevo = "Lunes";

    [ObservableProperty]
    private string horaInicioNueva = "18:00";

    [ObservableProperty]
    private string horaFinNueva = "20:00";

    [ObservableProperty]
    private string tipoSesionNueva = "teoria";

    [ObservableProperty]
    private int idLaboratorioNuevo;

    [ObservableProperty]
    private string horaInicioLabNueva = "18:00";

    [ObservableProperty]
    private string horaFinLabNueva = "20:00";

    [ObservableProperty]
    private decimal costoExtraNuevo;

    [ObservableProperty]
    private string? mensajeError;

    public SeccionListViewModel(
        ISeccionService seccionService,
        ICursoService cursoService,
        IPeriodoAcademicoService periodoService,
        ICatedraticoService catedraticoService,
        ISalonService salonService,
        ILaboratorioService laboratorioService,
        IDialogService dialogService)
    {
        _seccionService = seccionService;
        _cursoService = cursoService;
        _periodoService = periodoService;
        _catedraticoService = catedraticoService;
        _salonService = salonService;
        _laboratorioService = laboratorioService;
        _dialogService = dialogService;

        _ = CargarAsync();
        _ = CargarLaboratoriosDisponiblesAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        try
        {
            Secciones = new ObservableCollection<SeccionDto>(await _seccionService.GetAllAsync(true));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    private async Task CargarLaboratoriosDisponiblesAsync()
    {
        LaboratoriosDisponibles = new ObservableCollection<LaboratorioDto>(await _laboratorioService.GetAllAsync());
        if (LaboratoriosDisponibles.Count > 0)
        {
            IdLaboratorioNuevo = LaboratoriosDisponibles[0].Id;
        }
    }

    partial void OnSeccionSeleccionadaChanged(SeccionDto? value) => _ = CargarDetalleAsync();

    private async Task CargarDetalleAsync()
    {
        Horarios.Clear();
        LaboratoriosSeccion.Clear();
        if (SeccionSeleccionada is null)
        {
            return;
        }

        Horarios = new ObservableCollection<HorarioSeccionDto>(await _seccionService.GetHorariosAsync(SeccionSeleccionada.Id));
        LaboratoriosSeccion = new ObservableCollection<SeccionLaboratorioDto>(await _seccionService.GetLaboratoriosAsync(SeccionSeleccionada.Id));
    }

    [RelayCommand]
    private async Task NuevaSeccionAsync()
    {
        var editVm = new SeccionEditViewModel(_seccionService, _cursoService, _periodoService, _catedraticoService, _salonService, null);
        if (_dialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    private async Task EditarSeccionAsync()
    {
        if (SeccionSeleccionada is null)
        {
            return;
        }

        var editVm = new SeccionEditViewModel(_seccionService, _cursoService, _periodoService, _catedraticoService, _salonService, SeccionSeleccionada);
        if (_dialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    private async Task DesactivarSeccionAsync()
    {
        if (SeccionSeleccionada is null)
        {
            return;
        }

        var activarla = SeccionSeleccionada.Estado != EstadoConstantes.Activo;
        if (!_dialogService.Confirmar($"¿Desea {(activarla ? "activar" : "desactivar")} la sección '{SeccionSeleccionada.Codigo}'?"))
        {
            return;
        }

        try
        {
            await _seccionService.SetEstadoAsync(SeccionSeleccionada.Id, activarla);
            await CargarAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo completar la operación");
        }
    }

    [RelayCommand]
    private async Task AgregarHorarioAsync()
    {
        if (SeccionSeleccionada is null)
        {
            return;
        }

        if (!TimeSpan.TryParse(HoraInicioNueva, out var inicio) || !TimeSpan.TryParse(HoraFinNueva, out var fin))
        {
            _dialogService.MostrarMensaje("Formato de hora inválido. Use HH:mm.", "Datos inválidos");
            return;
        }

        try
        {
            await _seccionService.AgregarHorarioAsync(new HorarioSeccionDto
            {
                IdSeccion = SeccionSeleccionada.Id,
                DiaSemana = DiaSemanaNuevo,
                HoraInicio = inicio,
                HoraFin = fin,
                TipoSesion = TipoSesionNueva
            });
            await CargarDetalleAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo agregar el horario");
        }
    }

    [RelayCommand]
    private async Task QuitarHorarioAsync(HorarioSeccionDto? horario)
    {
        if (horario is null || !_dialogService.Confirmar("¿Quitar este horario?"))
        {
            return;
        }

        try
        {
            await _seccionService.QuitarHorarioAsync(horario.Id);
            await CargarDetalleAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo quitar el horario");
        }
    }

    [RelayCommand]
    private async Task AgregarLaboratorioAsync()
    {
        if (SeccionSeleccionada is null || IdLaboratorioNuevo == 0)
        {
            return;
        }

        if (!TimeSpan.TryParse(HoraInicioLabNueva, out var inicio) || !TimeSpan.TryParse(HoraFinLabNueva, out var fin))
        {
            _dialogService.MostrarMensaje("Formato de hora inválido. Use HH:mm.", "Datos inválidos");
            return;
        }

        try
        {
            await _seccionService.AgregarLaboratorioAsync(new SeccionLaboratorioDto
            {
                IdSeccion = SeccionSeleccionada.Id,
                IdLaboratorio = IdLaboratorioNuevo,
                DiaSemana = DiaSemanaNuevo,
                HoraInicio = inicio,
                HoraFin = fin,
                CostoExtra = CostoExtraNuevo
            });
            await CargarDetalleAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo agregar el laboratorio");
        }
    }

    [RelayCommand]
    private async Task QuitarLaboratorioAsync(SeccionLaboratorioDto? laboratorio)
    {
        if (laboratorio is null || !_dialogService.Confirmar("¿Quitar este laboratorio de la sección?"))
        {
            return;
        }

        try
        {
            await _seccionService.QuitarLaboratorioAsync(laboratorio.Id);
            await CargarDetalleAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo quitar el laboratorio");
        }
    }
}
