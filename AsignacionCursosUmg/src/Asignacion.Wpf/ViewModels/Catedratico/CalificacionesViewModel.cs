using System.Collections.ObjectModel;
using System.Linq;
using Asignacion.Services.Matricula;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Catedratico;

public partial class FilaCalificacionViewModel : ObservableObject
{
    public int IdDetalleAsignacion { get; }
    public string EstudianteCarnet { get; }
    public string EstudianteNombre { get; }
    public string? ResultadoActual { get; private set; }

    [ObservableProperty]
    private decimal? notaFinal;

    [ObservableProperty]
    private bool modificada;

    private bool _inicializando = true;

    public FilaCalificacionViewModel(DetalleAsignacionDto dto)
    {
        IdDetalleAsignacion = dto.Id;
        EstudianteCarnet = dto.EstudianteCarnet;
        EstudianteNombre = dto.EstudianteNombre;
        ResultadoActual = dto.Resultado;
        NotaFinal = dto.NotaFinal;
        _inicializando = false;
    }

    partial void OnNotaFinalChanged(decimal? value)
    {
        if (!_inicializando)
        {
            Modificada = true;
        }
    }
}

public partial class CalificacionesViewModel : ObservableObject
{
    private readonly IDetalleAsignacionService _service;
    private readonly CatedraticoContext _contexto;

    [ObservableProperty]
    private ObservableCollection<FilaCalificacionViewModel> filas = new();

    [ObservableProperty]
    private string? mensajeError;

    [ObservableProperty]
    private string? mensajeExito;

    [ObservableProperty]
    private bool guardando;

    public string SeccionCodigo => _contexto.SeccionCodigo;

    public CalificacionesViewModel(IDetalleAsignacionService service, CatedraticoContext contexto)
    {
        _service = service;
        _contexto = contexto;
        _ = CargarAsync();
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        MensajeError = null;
        MensajeExito = null;
        try
        {
            var roster = await _service.GetRosterBySeccionAsync(_contexto.IdSeccionSeleccionada);
            Filas = new ObservableCollection<FilaCalificacionViewModel>(roster.Select(d => new FilaCalificacionViewModel(d)));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand]
    private async Task GuardarCambiosAsync()
    {
        MensajeError = null;
        MensajeExito = null;
        var pendientes = Filas.Where(f => f.Modificada && f.NotaFinal is not null).ToList();
        if (pendientes.Count == 0)
        {
            MensajeError = "No hay calificaciones modificadas para guardar.";
            return;
        }

        Guardando = true;
        try
        {
            foreach (var fila in pendientes)
            {
                await _service.GuardarNotaAsync(fila.IdDetalleAsignacion, fila.NotaFinal!.Value);
            }

            MensajeExito = $"Se guardaron {pendientes.Count} calificación(es).";
            await CargarAsync();
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
        finally
        {
            Guardando = false;
        }
    }
}
