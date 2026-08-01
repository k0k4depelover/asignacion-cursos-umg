using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class FacultadEditViewModel : EditDialogViewModelBase
{
    private readonly IFacultadService _service;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private bool activo = true;

    public override string TituloDialogo => _id == 0 ? "Nueva facultad" : "Editar facultad";

    public FacultadEditViewModel(IFacultadService service, FacultadDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Nombre = existente.Nombre;
            Activo = existente.Estado == EstadoConstantes.Activo;
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
        var dto = new FacultadDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Nombre = Nombre.Trim(),
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
