using Asignacion.Data.Common;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class PeriodoAcademicoEditViewModel : EditDialogViewModelBase
{
    private readonly IPeriodoAcademicoService _service;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string? descripcion;

    [ObservableProperty]
    private string tipoPeriodo = "";

    [ObservableProperty]
    private DateTime? fechaInicio = DateTime.Today;

    [ObservableProperty]
    private DateTime? fechaFin = DateTime.Today;

    [ObservableProperty]
    private bool permiteInscripcion;

    [ObservableProperty]
    private bool permiteAsignacion;

    [ObservableProperty]
    private bool activo = true;

    public override string TituloDialogo => _id == 0 ? "Nuevo periodo académico" : "Editar periodo académico";

    public PeriodoAcademicoEditViewModel(IPeriodoAcademicoService service, PeriodoAcademicoDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Descripcion = existente.Descripcion;
            TipoPeriodo = existente.TipoPeriodo;
            FechaInicio = existente.FechaInicio;
            FechaFin = existente.FechaFin;
            PermiteInscripcion = existente.PermiteInscripcion;
            PermiteAsignacion = existente.PermiteAsignacion;
            Activo = existente.Estado == EstadoConstantes.Activo;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(TipoPeriodo))
        {
            error = "El código y el tipo de periodo son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new PeriodoAcademicoDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? null : Descripcion.Trim(),
            TipoPeriodo = TipoPeriodo.Trim(),
            FechaInicio = FechaInicio ?? DateTime.Today,
            FechaFin = FechaFin ?? DateTime.Today,
            PermiteInscripcion = PermiteInscripcion,
            PermiteAsignacion = PermiteAsignacion,
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo
        };

        if (_id == 0)
        {
            await _service.CreateAsync(dto);
        }
        else
        {
            await _service.UpdateAsync(dto);
        }
    }
}
