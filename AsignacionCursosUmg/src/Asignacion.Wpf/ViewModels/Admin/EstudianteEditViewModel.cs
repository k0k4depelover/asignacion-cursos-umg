using System.Collections.ObjectModel;
using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Personas;
using Asignacion.Wpf.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asignacion.Wpf.ViewModels.Admin;

public partial class EstudianteEditViewModel : EditDialogViewModelBase
{
    private readonly IEstudianteService _service;
    private readonly IUsuarioService _usuarioService;
    private readonly IPensumService _pensumService;
    private readonly int _id;

    [ObservableProperty]
    private string carnet = "";

    [ObservableProperty]
    private string dpi = "";

    [ObservableProperty]
    private string nombres = "";

    [ObservableProperty]
    private string apellidos = "";

    [ObservableProperty]
    private DateTime? fechaNacimiento = DateTime.Today;

    [ObservableProperty]
    private string? direccion;

    [ObservableProperty]
    private string? telefono;

    [ObservableProperty]
    private int cicloActual = 1;

    [ObservableProperty]
    private bool activo = true;

    [ObservableProperty]
    private int idUsuario;

    [ObservableProperty]
    private ObservableCollection<UsuarioDto> usuarios = new();

    [ObservableProperty]
    private int idPensum;

    [ObservableProperty]
    private ObservableCollection<PensumDto> pensums = new();

    public override string TituloDialogo => _id == 0 ? "Nuevo estudiante" : "Editar estudiante";

    public EstudianteEditViewModel(IEstudianteService service, IUsuarioService usuarioService, IPensumService pensumService, EstudianteDto? existente)
    {
        _service = service;
        _usuarioService = usuarioService;
        _pensumService = pensumService;
        _id = existente?.Id ?? 0;
        if (existente is not null)
        {
            Carnet = existente.Carnet;
            Dpi = existente.Dpi;
            Nombres = existente.Nombres;
            Apellidos = existente.Apellidos;
            FechaNacimiento = existente.FechaNacimiento;
            Direccion = existente.Direccion;
            Telefono = existente.Telefono;
            CicloActual = existente.CicloActual;
            Activo = existente.Estado == EstadoConstantes.Activo;
            IdUsuario = existente.IdUsuario;
            IdPensum = existente.IdPensum;
        }

        _ = CargarUsuariosAsync();
        _ = CargarPensumsAsync();
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

    private async Task CargarPensumsAsync()
    {
        var lista = await _pensumService.GetAllAsync(false);
        Pensums = new ObservableCollection<PensumDto>(lista);
        if (IdPensum == 0 && Pensums.Count > 0)
        {
            IdPensum = Pensums[0].Id;
        }
    }

    protected override bool Validar(out string? error)
    {
        if (string.IsNullOrWhiteSpace(Carnet) || string.IsNullOrWhiteSpace(Dpi) ||
            string.IsNullOrWhiteSpace(Nombres) || string.IsNullOrWhiteSpace(Apellidos))
        {
            error = "El carnet, el DPI, los nombres y los apellidos son obligatorios.";
            return false;
        }

        error = null;
        return true;
    }

    protected override async Task GuardarInternoAsync()
    {
        var dto = new EstudianteDto
        {
            Id = _id,
            Carnet = Carnet.Trim(),
            Dpi = Dpi.Trim(),
            Nombres = Nombres.Trim(),
            Apellidos = Apellidos.Trim(),
            FechaNacimiento = FechaNacimiento ?? DateTime.Today,
            Direccion = string.IsNullOrWhiteSpace(Direccion) ? null : Direccion.Trim(),
            Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono.Trim(),
            CicloActual = CicloActual,
            Estado = Activo ? EstadoConstantes.Activo : EstadoConstantes.Inactivo,
            IdUsuario = IdUsuario,
            IdPensum = IdPensum
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
