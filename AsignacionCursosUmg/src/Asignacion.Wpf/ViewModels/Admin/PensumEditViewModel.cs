using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class PensumEditViewModel : EditDialogViewModelBase
{
    private readonly IPensumService _pensumService;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private int anio = DateTime.Today.Year;

    [ObservableProperty]
    private string jornada = "";

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idCarrera;

    [ObservableProperty]
    private ObservableCollection<CarreraDto> carreras = new();

    public override string TituloDialogo => _id == 0 ? "Nuevo pensum" : "Editar pensum";

    public PensumEditViewModel(IPensumService pensumService, ICarreraService carreraService, PensumDto? existente)
    {
        _pensumService = pensumService;
        _id = existente?.Id ?? 0;

        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Anio = existente.Anio;
            Jornada = existente.Jornada;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdCarrera = existente.IdCarrera;
        }

        _ = CargarCarrerasAsync(carreraService);
    }

    private async Task CargarCarrerasAsync(ICarreraService carreraService)
    {
        Carreras = new ObservableCollection<CarreraDto>(await carreraService.GetAllAsync());
        if (IdCarrera == 0 && Carreras.Count > 0)
        {
            IdCarrera = Carreras[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Jornada) || IdCarrera == 0)
        {
            error = "Código, jornada y carrera son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new PensumDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Anio = Anio,
            Jornada = Jornada.Trim(),
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdCarrera = IdCarrera
        };

        if (_id == 0)
        {
            await _pensumService.CreateAsync(dto);
        }
        else
        {
            await _pensumService.UpdateAsync(dto);
        }
    }
}
