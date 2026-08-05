using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class UsuarioEditViewModel : EditDialogViewModelBase
{
    private readonly IUsuarioService _service;
    private readonly IRolService _rolService;
    private readonly int _id;

    [ObservableProperty]
    private string nombreUsuario = "";

    [ObservableProperty]
    private string correoLogin = "";

    [ObservableProperty]
    private string? correoRecuperacion;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idRol;

    [ObservableProperty]
    private ObservableCollection<RolDto> roles = new();

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private bool esNuevo;

    [ObservableProperty]
    private string? passwordTemporalGenerada;

    public override string TituloDialogo => _id == 0 ? "Nuevo usuario" : "Editar usuario";

    public UsuarioEditViewModel(IUsuarioService service, IRolService rolService, UsuarioDto? existente)
    {
        _service = service;
        _rolService = rolService;
        _id = existente?.Id ?? 0;
        EsNuevo = existente is null;
        if (existente is not null)
        {
            NombreUsuario = existente.NombreUsuario;
            CorreoLogin = existente.CorreoLogin;
            CorreoRecuperacion = existente.CorreoRecuperacion;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdRol = existente.IdRol;
        }

        _ = CargarRolesAsync();
    }

    private async Task CargarRolesAsync()
    {
        var lista = await _rolService.GetAllAsync(false);
        Roles = new ObservableCollection<RolDto>(lista);
        if (IdRol == 0 && Roles.Count > 0)
        {
            IdRol = Roles[0].Id;
        }
    }

    [RelayCommand]
    private async Task ResetearPasswordAsync()
    {
        MensajeError = null;
        try
        {
            var temporal = await _service.ResetearPasswordAsync(_id);
            PasswordTemporalGenerada = $"Nueva contraseña temporal: {temporal}, comuníquela al usuario.";
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(CorreoLogin))
        {
            error = "El nombre de usuario y el correo de acceso son obligatorios.";
            return false;
        }

        if (EsNuevo && string.IsNullOrWhiteSpace(Password))
        {
            error = "Debe ingresar una contraseña inicial.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new UsuarioDto
        {
            Id = _id,
            NombreUsuario = NombreUsuario.Trim(),
            CorreoLogin = CorreoLogin.Trim(),
            CorreoRecuperacion = string.IsNullOrWhiteSpace(CorreoRecuperacion) ? null : CorreoRecuperacion.Trim(),
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdRol = IdRol
        };

        if (EsNuevo)
        {
            await _service.CreateAsync(dto, Password);
        }
        else
        {
            await _service.UpdateAsync(dto);
        }
    }
}
