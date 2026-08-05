using Asignacion.Services.Reportes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class AdminDashboardViewModel : ObservableObject
{
    private readonly IReporteService _service;

    [ObservableProperty]
    private AdminDashboardDto? datos;

    [ObservableProperty]
    private string? mensajeError;

    public AdminDashboardViewModel(IReporteService service)
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
            Datos = await _service.GetDashboardAsync();
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
