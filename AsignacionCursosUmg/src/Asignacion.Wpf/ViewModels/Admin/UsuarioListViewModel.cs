using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class UsuarioListViewModel : CrudListViewModel<UsuarioDto>
{
    private readonly IUsuarioService _service;
    private readonly IRolService _rolService;

    public UsuarioListViewModel(IUsuarioService service, IRolService rolService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _rolService = rolService;
        CargarAlIniciar();
    }

    protected override Task<List<UsuarioDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(UsuarioDto? existente) => new UsuarioEditViewModel(_service, _rolService, existente);

    protected override Task DesactivarInternoAsync(UsuarioDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(UsuarioDto item) => item.NombreUsuario;
}
