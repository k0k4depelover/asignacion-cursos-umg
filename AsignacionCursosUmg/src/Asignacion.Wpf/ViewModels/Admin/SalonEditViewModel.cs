using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class SalonEditViewModel : EditDialogViewModelBase
{
    private readonly ISalonService _service;
    private readonly IEdificioService _edificioService;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private int capacidad;

    [ObservableProperty]
    private string tipoEspacio = "";

    [ObservableProperty]
    private int nivel;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idEdificio;

    [ObservableProperty]
    private ObservableCollection<EdificioDto> edificios = new();

    public override string TituloDialogo => _id == 0 ? "Nuevo salón" : "Editar salón";

    public SalonEditViewModel(ISalonService service, IEdificioService edificioService, SalonDto? existente)
    {
        _service = service;
        _edificioService = edificioService;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Nombre = existente.Nombre;
            Capacidad = existente.Capacidad;
            TipoEspacio = existente.TipoEspacio;
            Nivel = existente.Nivel;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdEdificio = existente.IdEdificio;
        }

        _ = CargarEdificiosAsync();
    }

    private async Task CargarEdificiosAsync()
    {
        var lista = await _edificioService.GetAllAsync(false);
        Edificios = new ObservableCollection<EdificioDto>(lista);
        if (IdEdificio == 0 && Edificios.Count > 0)
        {
            IdEdificio = Edificios[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(TipoEspacio))
        {
            error = "El código, el nombre y el tipo de espacio son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new SalonDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Nombre = Nombre.Trim(),
            Capacidad = Capacidad,
            TipoEspacio = TipoEspacio.Trim(),
            Nivel = Nivel,
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdEdificio = IdEdificio
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
