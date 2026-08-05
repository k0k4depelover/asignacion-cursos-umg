using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class EdificioEditViewModel : EditDialogViewModelBase
{
    private readonly IEdificioService _service;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string sede = "";

    [ObservableProperty]
    private string? ubicacion;

    [ObservableProperty]
    private bool activo = true;

    public override string TituloDialogo => _id == 0 ? "Nuevo edificio" : "Editar edificio";

    public EdificioEditViewModel(IEdificioService service, EdificioDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Nombre = existente.Nombre;
            Sede = existente.Sede;
            Ubicacion = existente.Ubicacion;
            Activo = existente.Estado == EstadoConstantes.Activo;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Sede))
        {
            error = "El código, el nombre y la sede son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new EdificioDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Nombre = Nombre.Trim(),
            Sede = Sede.Trim(),
            Ubicacion = string.IsNullOrWhiteSpace(Ubicacion) ? null : Ubicacion.Trim(),
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
