using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Common;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class PensumListViewModel : ObservableObject
{
    private readonly IPensumService _pensumService;
    private readonly ICarreraService _carreraService;
    private readonly ICursoService _cursoService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ObservableCollection<PensumDto> pensums = new();

    [ObservableProperty]
    private PensumDto? pensumSeleccionado;

    [ObservableProperty]
    private ObservableCollection<PensumCursoDto> cursosDelPensum = new();

    [ObservableProperty]
    private PensumCursoDto? cursoSeleccionado;

    [ObservableProperty]
    private ObservableCollection<RequisitoCursoDto> requisitosDelCurso = new();

    [ObservableProperty]
    private ObservableCollection<CursoDto> cursosDisponibles = new();

    [ObservableProperty]
    private int idCursoNuevo;

    [ObservableProperty]
    private int cicloNuevo = 1;

    [ObservableProperty]
    private bool obligatorioNuevo = true;

    [ObservableProperty]
    private string tipoRequisitoNuevo = EstadoConstantes.TipoRequisitoCursoAprobado;

    [ObservableProperty]
    private int idCursoRequeridoNuevo;

    [ObservableProperty]
    private int? creditosMinimosNuevo;

    [ObservableProperty]
    private string? descripcionRequisitoNuevo;

    [ObservableProperty]
    private string? mensajeError;

    public PensumListViewModel(IPensumService pensumService, ICarreraService carreraService, ICursoService cursoService, IDialogService dialogService)
    {
        _pensumService = pensumService;
        _carreraService = carreraService;
        _cursoService = cursoService;
        _dialogService = dialogService;
        _ = CargarAsync();
        _ = CargarCursosDisponiblesAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        try
        {
            Pensums = new ObservableCollection<PensumDto>(await _pensumService.GetAllAsync());
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    private async Task CargarCursosDisponiblesAsync()
    {
        CursosDisponibles = new ObservableCollection<CursoDto>(await _cursoService.GetAllAsync());
        if (CursosDisponibles.Count > 0)
        {
            IdCursoNuevo = CursosDisponibles[0].Id;
            IdCursoRequeridoNuevo = CursosDisponibles[0].Id;
        }
    }

    partial void OnPensumSeleccionadoChanged(PensumDto? value) => _ = CargarCursosDelPensumAsync();

    private async Task CargarCursosDelPensumAsync()
    {
        CursosDelPensum.Clear();
        RequisitosDelCurso.Clear();
        CursoSeleccionado = null;
        if (PensumSeleccionado is null)
        {
            return;
        }

        CursosDelPensum = new ObservableCollection<PensumCursoDto>(await _pensumService.GetCursosAsync(PensumSeleccionado.Id));
    }

    partial void OnCursoSeleccionadoChanged(PensumCursoDto? value) => _ = CargarRequisitosAsync();

    private async Task CargarRequisitosAsync()
    {
        RequisitosDelCurso.Clear();
        if (CursoSeleccionado is null)
        {
            return;
        }

        RequisitosDelCurso = new ObservableCollection<RequisitoCursoDto>(await _pensumService.GetRequisitosAsync(CursoSeleccionado.Id));
    }

    [RelayCommand]
    private async Task NuevoPensumAsync()
    {
        var editVm = new PensumEditViewModel(_pensumService, _carreraService, null);
        if (_dialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    private async Task EditarPensumAsync()
    {
        if (PensumSeleccionado is null)
        {
            return;
        }

        var editVm = new PensumEditViewModel(_pensumService, _carreraService, PensumSeleccionado);
        if (_dialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    private async Task DesactivarPensumAsync()
    {
        if (PensumSeleccionado is null)
        {
            return;
        }

        var activarlo = PensumSeleccionado.Estado != EstadoConstantes.Activo;
        if (!_dialogService.Confirmar($"¿Desea {(activarlo ? "activar" : "desactivar")} el pensum '{PensumSeleccionado.Codigo}'?"))
        {
            return;
        }

        try
        {
            await _pensumService.SetEstadoAsync(PensumSeleccionado.Id, activarlo);
            await CargarAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo completar la operación");
        }
    }

    [RelayCommand]
    private async Task AgregarCursoAsync()
    {
        if (PensumSeleccionado is null || IdCursoNuevo == 0)
        {
            return;
        }

        try
        {
            await _pensumService.AgregarCursoAsync(PensumSeleccionado.Id, IdCursoNuevo, CicloNuevo, ObligatorioNuevo);
            await CargarCursosDelPensumAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo agregar el curso");
        }
    }

    [RelayCommand]
    private async Task QuitarCursoAsync()
    {
        if (CursoSeleccionado is null)
        {
            return;
        }

        if (!_dialogService.Confirmar($"¿Quitar '{CursoSeleccionado.CursoNombre}' de este pensum?"))
        {
            return;
        }

        try
        {
            await _pensumService.QuitarCursoAsync(CursoSeleccionado.Id);
            await CargarCursosDelPensumAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo quitar el curso");
        }
    }

    [RelayCommand]
    private async Task AgregarRequisitoAsync()
    {
        if (CursoSeleccionado is null || IdCursoRequeridoNuevo == 0)
        {
            return;
        }

        try
        {
            await _pensumService.AgregarRequisitoAsync(new RequisitoCursoDto
            {
                IdPensumCurso = CursoSeleccionado.Id,
                TipoRequisito = TipoRequisitoNuevo,
                IdCursoRequerido = IdCursoRequeridoNuevo,
                CreditosMinimos = CreditosMinimosNuevo,
                Descripcion = DescripcionRequisitoNuevo
            });
            await CargarRequisitosAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo agregar el requisito");
        }
    }

    [RelayCommand]
    private async Task QuitarRequisitoAsync(RequisitoCursoDto? requisito)
    {
        if (requisito is null || !_dialogService.Confirmar("¿Quitar este requisito?"))
        {
            return;
        }

        try
        {
            await _pensumService.QuitarRequisitoAsync(requisito.Id);
            await CargarRequisitosAsync();
        }
        catch (ServiceException ex)
        {
            _dialogService.MostrarMensaje(ex.Message, "No se pudo quitar el requisito");
        }
    }
}
