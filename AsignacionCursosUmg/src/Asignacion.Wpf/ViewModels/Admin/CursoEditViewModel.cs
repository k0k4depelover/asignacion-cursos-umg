using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class CursoEditViewModel : EditDialogViewModelBase
{
    private readonly ICursoService _service;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private int creditos;

    [ObservableProperty]
    private bool requiereLaboratorio;

    [ObservableProperty]
    private bool activo = true;

    public override string TituloDialogo => _id == 0 ? "Nuevo curso" : "Editar curso";

    public CursoEditViewModel(ICursoService service, CursoDto? existente)
    {
        _service = service;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Nombre = existente.Nombre;
            Creditos = existente.Creditos;
            RequiereLaboratorio = existente.RequiereLaboratorio;
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
        var dto = new CursoDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Nombre = Nombre.Trim(),
            Creditos = Creditos,
            RequiereLaboratorio = RequiereLaboratorio,
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
