using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class CarreraEditViewModel : EditDialogViewModelBase
{
    private readonly ICarreraService _service;
    private readonly IFacultadService _facultadService;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private int totalCiclos;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idFacultad;

    [ObservableProperty]
    private ObservableCollection<FacultadDto> facultades = new();

    public override string TituloDialogo => _id == 0 ? "Nueva carrera" : "Editar carrera";

    public CarreraEditViewModel(ICarreraService service, IFacultadService facultadService, CarreraDto? existente)
    {
        _service = service;
        _facultadService = facultadService;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Nombre = existente.Nombre;
            TotalCiclos = existente.TotalCiclos;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdFacultad = existente.IdFacultad;
        }

        _ = CargarFacultadesAsync();
    }

    private async Task CargarFacultadesAsync()
    {
        var lista = await _facultadService.GetAllAsync(false);
        Facultades = new ObservableCollection<FacultadDto>(lista);
        if (IdFacultad == 0 && Facultades.Count > 0)
        {
            IdFacultad = Facultades[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre))
        {
            error = "El código y el nombre son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new CarreraDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Nombre = Nombre.Trim(),
            TotalCiclos = TotalCiclos,
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdFacultad = IdFacultad
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
