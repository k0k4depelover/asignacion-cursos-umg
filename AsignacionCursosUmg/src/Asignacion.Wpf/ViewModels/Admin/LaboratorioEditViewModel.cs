using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class LaboratorioEditViewModel : EditDialogViewModelBase
{
    private readonly ILaboratorioService _service;
    private readonly ISalonService _salonService;
    private readonly int _id;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string? descripcion;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idSalon;

    [ObservableProperty]
    private ObservableCollection<SalonDto> salones = new();

    public override string TituloDialogo => _id == 0 ? "Nuevo laboratorio" : "Editar laboratorio";

    public LaboratorioEditViewModel(ILaboratorioService service, ISalonService salonService, LaboratorioDto? existente)
    {
        _service = service;
        _salonService = salonService;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Nombre = existente.Nombre;
            Descripcion = existente.Descripcion;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdSalon = existente.IdSalon;
        }

        _ = CargarSalonesAsync();
    }

    private async Task CargarSalonesAsync()
    {
        var lista = await _salonService.GetAllAsync(false);
        Salones = new ObservableCollection<SalonDto>(lista);
        if (IdSalon == 0 && Salones.Count > 0)
        {
            IdSalon = Salones[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            error = "El nombre es obligatorio.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new LaboratorioDto
        {
            Id = _id,
            Nombre = Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? null : Descripcion.Trim(),
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdSalon = IdSalon
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
