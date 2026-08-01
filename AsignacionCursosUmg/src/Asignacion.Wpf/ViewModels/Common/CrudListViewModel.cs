using System.Collections.ObjectModel;
using Asignacion.Services.Common;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Common;

/// <summary>
/// Shared shape for the ~generic admin CRUD screens (one DataGrid + toolbar + soft-delete
/// toggle). Concrete subclasses only need to say how to load/create/edit/retire a single
/// DTO type — see FacultadListViewModel for the reference implementation of the pattern.
/// </summary>
public abstract partial class CrudListViewModel<TDto> : ObservableObject where TDto : class
{
    protected readonly IDialogService DialogService;

    [ObservableProperty]
    private ObservableCollection<TDto> items = new();

    [ObservableProperty]
    private TDto? selected;

    [ObservableProperty]
    private bool incluirInactivos;

    [ObservableProperty]
    private string? mensajeError;

    protected CrudListViewModel(IDialogService dialogService)
    {
        DialogService = dialogService;
    }

    protected void CargarAlIniciar() => _ = CargarAsync();

    [RelayCommand]
    protected async Task CargarAsync()
    {
        MensajeError = null;
        try
        {
            var lista = await ObtenerItemsAsync(IncluirInactivos);
            Items = new ObservableCollection<TDto>(lista);
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    partial void OnIncluirInactivosChanged(bool value) => _ = CargarAsync();

    [RelayCommand]
    private async Task NuevoAsync()
    {
        var editVm = CrearViewModelEdicion(null);
        if (DialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand(CanExecute = nameof(HaySeleccion))]
    private async Task EditarAsync()
    {
        if (Selected is null)
        {
            return;
        }

        var editVm = CrearViewModelEdicion(Selected);
        if (DialogService.ShowEditDialog(editVm))
        {
            await CargarAsync();
        }
    }

    [RelayCommand(CanExecute = nameof(HaySeleccion))]
    private async Task EliminarAsync()
    {
        if (Selected is null)
        {
            return;
        }

        if (!DialogService.Confirmar($"¿Desea desactivar '{DescribirItem(Selected)}'?"))
        {
            return;
        }

        try
        {
            await DesactivarInternoAsync(Selected);
            await CargarAsync();
        }
        catch (ServiceException ex)
        {
            DialogService.MostrarMensaje(ex.Message, "No se pudo completar la operación");
        }
    }

    private bool HaySeleccion() => Selected is not null;

    partial void OnSelectedChanged(TDto? value)
    {
        EditarCommand.NotifyCanExecuteChanged();
        EliminarCommand.NotifyCanExecuteChanged();
    }

    protected abstract Task<List<TDto>> ObtenerItemsAsync(bool incluirInactivos);
    protected abstract IEditDialogViewModel CrearViewModelEdicion(TDto? existente);
    protected abstract Task DesactivarInternoAsync(TDto item);
    protected abstract string DescribirItem(TDto item);
}
