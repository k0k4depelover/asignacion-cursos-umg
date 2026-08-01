using Asignacion.Services.Personas;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class PermisoEditViewModel : EditDialogViewModelBase
{
    private readonly IPermisoService _service;
    private readonly int _id;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string? descripcion;

    public override string TituloDialogo => _id == 0 ? "Nuevo permiso" : "Editar permiso";

    public PermisoEditViewModel(IPermisoService service, PermisoDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Nombre = existente.Nombre;
            Descripcion = existente.Descripcion;
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
        var dto = new PermisoDto
        {
            Id = _id,
            Nombre = Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? null : Descripcion.Trim()
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
