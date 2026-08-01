using System.Collections.ObjectModel;
using System.Linq;
using Asignacion.Services.Matricula;
using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Estudiante;

public class BloqueHorarioViewModel
{
    public required string Curso { get; init; }
    public required string DiaSemana { get; init; }
    public required string Horario { get; init; }
    public required string Tipo { get; init; }
}

public partial class MiHorarioViewModel : ObservableObject
{
    private readonly IInscripcionService _inscripcionService;
    private readonly IMatriculaWorkflowService _matriculaService;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private ObservableCollection<BloqueHorarioViewModel> bloques = new();

    [ObservableProperty]
    private string? mensajeError;

    public MiHorarioViewModel(IInscripcionService inscripcionService, IMatriculaWorkflowService matriculaService, CurrentSessionService session)
    {
        _inscripcionService = inscripcionService;
        _matriculaService = matriculaService;
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
            var inscripciones = await _inscripcionService.GetByEstudianteAsync(idEstudiante.Value);
            var actual = inscripciones.FirstOrDefault();
            if (actual is null)
            {
                MensajeError = "No tiene inscripciones registradas.";
                return;
            }

            var secciones = await _matriculaService.GetMiHorarioAsync(idEstudiante.Value, actual.IdPeriodo);

            var lista = new List<BloqueHorarioViewModel>();
            foreach (var seccion in secciones)
            {
                lista.AddRange(seccion.Horarios.Select(h => new BloqueHorarioViewModel
                {
                    Curso = seccion.Codigo,
                    DiaSemana = h.DiaSemana,
                    Horario = $"{h.HoraInicio:hh\\:mm} - {h.HoraFin:hh\\:mm}",
                    Tipo = "Teoría"
                }));
                lista.AddRange(seccion.HorariosLaboratorio.Select(h => new BloqueHorarioViewModel
                {
                    Curso = seccion.Codigo,
                    DiaSemana = h.DiaSemana,
                    Horario = $"{h.HoraInicio:hh\\:mm} - {h.HoraFin:hh\\:mm}",
                    Tipo = "Laboratorio"
                }));
            }

            Bloques = new ObservableCollection<BloqueHorarioViewModel>(lista.OrderBy(b => b.DiaSemana).ThenBy(b => b.Horario));
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }
}
