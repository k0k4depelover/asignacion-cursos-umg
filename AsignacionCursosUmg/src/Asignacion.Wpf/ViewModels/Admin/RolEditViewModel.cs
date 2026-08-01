using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class RolEditViewModel : EditDialogViewModelBase
{
    private readonly IRolService _service;
    private readonly int _id;

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string? descripcion;

    [ObservableProperty]
    private bool activo = true;

    public override string TituloDialogo => _id == 0 ? "Nuevo rol" : "Editar rol";

    public RolEditViewModel(IRolService service, RolDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Nombre = existente.Nombre;
            Descripcion = existente.Descripcion;
            Activo = existente.Estado == EstadoConstantes.Activo;
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
        var dto = new RolDto
        {
            Id = _id,
            Nombre = Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? null : Descripcion.Trim(),
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
