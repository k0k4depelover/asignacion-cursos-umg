using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Infraestructura;
using Asignacion.Services.Personas;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class SeccionEditViewModel : EditDialogViewModelBase
{
    private readonly ISeccionService _seccionService;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string jornada = "";

    [ObservableProperty]
    private int cupoMaximo = 30;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idCurso;

    [ObservableProperty]
    private int idPeriodo;

    [ObservableProperty]
    private int idCatedratico;

    [ObservableProperty]
    private int idSalon;

    [ObservableProperty]
    private ObservableCollection<CursoDto> cursos = new();

    [ObservableProperty]
    private ObservableCollection<PeriodoAcademicoDto> periodos = new();

    [ObservableProperty]
    private ObservableCollection<CatedraticoDto> catedraticos = new();

    [ObservableProperty]
    private ObservableCollection<SalonDto> salones = new();

    public override string TituloDialogo => _id == 0 ? "Nueva sección" : "Editar sección";

    public SeccionEditViewModel(
        ISeccionService seccionService,
        ICursoService cursoService,
        IPeriodoAcademicoService periodoService,
        ICatedraticoService catedraticoService,
        ISalonService salonService,
        SeccionDto? existente)
    {
        _seccionService = seccionService;
        _id = existente?.Id ?? 0;

        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Jornada = existente.Jornada;
            CupoMaximo = existente.CupoMaximo;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdCurso = existente.IdCurso;
            IdPeriodo = existente.IdPeriodo;
            IdCatedratico = existente.IdCatedratico;
            IdSalon = existente.IdSalon;
        }

        _ = CargarListasAsync(cursoService, periodoService, catedraticoService, salonService);
    }

    private async Task CargarListasAsync(
        ICursoService cursoService,
        IPeriodoAcademicoService periodoService,
        ICatedraticoService catedraticoService,
        ISalonService salonService)
    {
        Cursos = new ObservableCollection<CursoDto>(await cursoService.GetAllAsync());
        Periodos = new ObservableCollection<PeriodoAcademicoDto>(await periodoService.GetAllAsync());
        Catedraticos = new ObservableCollection<CatedraticoDto>(await catedraticoService.GetAllAsync());
        Salones = new ObservableCollection<SalonDto>(await salonService.GetAllAsync());

        if (IdCurso == 0 && Cursos.Count > 0) IdCurso = Cursos[0].Id;
        if (IdPeriodo == 0 && Periodos.Count > 0) IdPeriodo = Periodos[0].Id;
        if (IdCatedratico == 0 && Catedraticos.Count > 0) IdCatedratico = Catedraticos[0].Id;
        if (IdSalon == 0 && Salones.Count > 0) IdSalon = Salones[0].Id;
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Jornada) || CupoMaximo <= 0
            || IdCurso == 0 || IdPeriodo == 0 || IdCatedratico == 0 || IdSalon == 0)
        {
            error = "Complete todos los campos (código, jornada, cupo, curso, período, catedrático y salón).";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new SeccionDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Jornada = Jornada.Trim(),
            CupoMaximo = CupoMaximo,
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdCurso = IdCurso,
            IdPeriodo = IdPeriodo,
            IdCatedratico = IdCatedratico,
            IdSalon = IdSalon
        };

        if (_id == 0)
        {
            await _seccionService.CreateAsync(dto);
        }
        else
        {
            await _seccionService.UpdateAsync(dto);
        }
    }
}
