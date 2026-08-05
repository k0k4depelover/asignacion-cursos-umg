using System.Collections.ObjectModel;
using System.Linq;
using Asignacion.Services.Common;
using Asignacion.Services.Matricula;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Estudiante;

public partial class CursoElegibleSeleccionableViewModel(CursoElegibleDto curso) : ObservableObject
{
    public CursoElegibleDto Curso { get; } = curso;

    [ObservableProperty]
    private bool seleccionado;

    [ObservableProperty]
    private int idSeccionSeleccionada;

    [ObservableProperty]
    private ObservableCollection<SeccionInscribibleDto> secciones = new();
}

public partial class InscripcionWizardViewModel : ObservableObject
{
    private readonly IPeriodoAcademicoService _periodoService;
    private readonly IMatriculaWorkflowService _matriculaService;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private ObservableCollection<PeriodoAcademicoDto> periodosDisponibles = new();

    [ObservableProperty]
    private int idPeriodoSeleccionado;

    [ObservableProperty]
    private ObservableCollection<CursoElegibleSeleccionableViewModel> cursosElegibles = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrandoFormularioCursos))]
    private bool mostrandoCursos;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrandoResultado))]
    [NotifyPropertyChangedFor(nameof(MostrandoFormularioCursos))]
    private ResultadoInscripcionDto? resultado;

    public bool MostrandoResultado => Resultado is not null;
    public bool MostrandoFormularioCursos => MostrandoCursos && Resultado is null;

    [ObservableProperty]
    private string? mensajeError;

    [ObservableProperty]
    private bool inscribiendo;

    public InscripcionWizardViewModel(IPeriodoAcademicoService periodoService, IMatriculaWorkflowService matriculaService, CurrentSessionService session)
    {
        _periodoService = periodoService;
        _matriculaService = matriculaService;
        _session = session;
        _ = CargarPeriodosAsync();
    }

    private async Task CargarPeriodosAsync()
    {
        PeriodosDisponibles = new ObservableCollection<PeriodoAcademicoDto>(await _periodoService.GetAbiertosParaInscripcionAsync());
        if (PeriodosDisponibles.Count > 0)
        {
            IdPeriodoSeleccionado = PeriodosDisponibles[0].Id;
        }
    }

    [RelayCommand]
    private async Task BuscarCursosAsync()
    {
        MensajeError = null;
        Resultado = null;
        var idEstudiante = _session.Current?.IdEstudiante;
        if (idEstudiante is null || IdPeriodoSeleccionado == 0)
        {
            MensajeError = "Seleccione un período válido.";
            return;
        }

        try
        {
            var elegibles = await _matriculaService.GetCursosElegiblesAsync(idEstudiante.Value, IdPeriodoSeleccionado);
            var envoltorios = elegibles.Select(c => new CursoElegibleSeleccionableViewModel(c)).ToList();

            foreach (var envoltorio in envoltorios.Where(e => e.Curso.CumpleRequisitos))
            {
                var secciones = await _matriculaService.GetSeccionesDisponiblesAsync(IdPeriodoSeleccionado, envoltorio.Curso.IdCurso);
                envoltorio.Secciones = new ObservableCollection<SeccionInscribibleDto>(secciones);
                if (secciones.Count > 0)
                {
                    envoltorio.IdSeccionSeleccionada = secciones[0].IdSeccion;
                }
            }

            CursosElegibles = new ObservableCollection<CursoElegibleSeleccionableViewModel>(envoltorios);
            MostrandoCursos = true;
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand]
    private async Task ConfirmarInscripcionAsync()
    {
        MensajeError = null;
        var idEstudiante = _session.Current?.IdEstudiante;
        if (idEstudiante is null)
        {
            return;
        }

        var seleccionadas = CursosElegibles
            .Where(c => c.Seleccionado && c.IdSeccionSeleccionada != 0)
            .Select(c => c.IdSeccionSeleccionada)
            .ToList();

        if (seleccionadas.Count == 0)
        {
            MensajeError = "Seleccione al menos un curso con su sección.";
            return;
        }

        Inscribiendo = true;
        try
        {
            Resultado = await _matriculaService.InscribirAsync(idEstudiante.Value, IdPeriodoSeleccionado, seleccionadas);
            MostrandoCursos = false;
        }
        catch (ServiceException ex)
        {
            MensajeError = ex.Message;
        }
        finally
        {
            Inscribiendo = false;
        }
    }

    [RelayCommand]
    private void NuevaInscripcion()
    {
        Resultado = null;
        MostrandoCursos = false;
        CursosElegibles.Clear();
        MensajeError = null;
    }
}
