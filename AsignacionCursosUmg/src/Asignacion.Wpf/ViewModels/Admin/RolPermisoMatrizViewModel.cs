using System.Collections.ObjectModel;
using Asignacion.Services.Personas;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class CeldaAsignacionViewModel(IRolPermisoService service, int idRol, int idPermiso, bool asignadoInicial) : ObservableObject
{
    public int IdRol => idRol;
    public int IdPermiso => idPermiso;

    [ObservableProperty]
    private bool asignado = asignadoInicial;

    private bool _actualizando;

    partial void OnAsignadoChanged(bool value)
    {
        if (_actualizando)
        {
            return;
        }

        _actualizando = true;
        _ = ActualizarAsync(value);
    }

    private async Task ActualizarAsync(bool asignado)
    {
        try
        {
            if (asignado)
            {
                await service.AsignarAsync(idRol, idPermiso);
            }
            else
            {
                await service.QuitarAsync(idRol, idPermiso);
            }
        }
        finally
        {
            _actualizando = false;
        }
    }
}

public class FilaPermisoViewModel
{
    public required string NombrePermiso { get; init; }
    public ObservableCollection<CeldaAsignacionViewModel> Celdas { get; init; } = new();
}

public partial class RolPermisoMatrizViewModel : ObservableObject
{
    private readonly IRolPermisoService _service;

    [ObservableProperty]
    private ObservableCollection<RolResumenDto> roles = new();

    [ObservableProperty]
    private ObservableCollection<FilaPermisoViewModel> filas = new();

    [ObservableProperty]
    private string? mensajeError;

    public RolPermisoMatrizViewModel(IRolPermisoService service)
    {
        _service = service;
        _ = CargarAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        try
        {
            var matriz = await _service.GetMatrizAsync();
            Roles = new ObservableCollection<RolResumenDto>(matriz.Roles);

            Filas = new ObservableCollection<FilaPermisoViewModel>(matriz.Permisos.Select(permiso => new FilaPermisoViewModel
            {
                NombrePermiso = permiso.Nombre,
                Celdas = new ObservableCollection<CeldaAsignacionViewModel>(matriz.Roles.Select(rol =>
                    new CeldaAsignacionViewModel(_service, rol.Id, permiso.Id, matriz.Asignados.Contains((rol.Id, permiso.Id)))))
            }));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
