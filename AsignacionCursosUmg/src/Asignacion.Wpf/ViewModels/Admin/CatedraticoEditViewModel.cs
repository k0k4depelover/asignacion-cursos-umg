using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class CatedraticoEditViewModel : EditDialogViewModelBase
{
    private readonly ICatedraticoService _service;
    private readonly IUsuarioService _usuarioService;
    private readonly int _id;

    [ObservableProperty]
    private string codigo = "";

    [ObservableProperty]
    private string dpi = "";

    [ObservableProperty]
    private string nombres = "";

    [ObservableProperty]
    private string apellidos = "";

    [ObservableProperty]
    private string? telefono;

    [ObservableProperty]
    private string? profesion;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idUsuario;

    [ObservableProperty]
    private ObservableCollection<UsuarioDto> usuarios = new();

    public override string TituloDialogo => _id == 0 ? "Nuevo catedrático" : "Editar catedrático";

    public CatedraticoEditViewModel(ICatedraticoService service, IUsuarioService usuarioService, CatedraticoDto? existente)
    {
        _service = service;
        _usuarioService = usuarioService;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Codigo = existente.Codigo;
            Dpi = existente.Dpi;
            Nombres = existente.Nombres;
            Apellidos = existente.Apellidos;
            Telefono = existente.Telefono;
            Profesion = existente.Profesion;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdUsuario = existente.IdUsuario;
        }

        _ = CargarUsuariosAsync();
    }

    private async Task CargarUsuariosAsync()
    {
        var lista = await _usuarioService.GetAllAsync(false);
        Usuarios = new ObservableCollection<UsuarioDto>(lista);
        if (IdUsuario == 0 && Usuarios.Count > 0)
        {
            IdUsuario = Usuarios[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Dpi) ||
            string.IsNullOrWhiteSpace(Nombres) || string.IsNullOrWhiteSpace(Apellidos))
        {
            error = "El código, el DPI, los nombres y los apellidos son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new CatedraticoDto
        {
            Id = _id,
            Codigo = Codigo.Trim(),
            Dpi = Dpi.Trim(),
            Nombres = Nombres.Trim(),
            Apellidos = Apellidos.Trim(),
            Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono.Trim(),
            Profesion = string.IsNullOrWhiteSpace(Profesion) ? null : Profesion.Trim(),
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdUsuario = IdUsuario
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
