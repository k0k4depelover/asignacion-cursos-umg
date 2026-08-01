using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class CatedraticoListViewModel : CrudListViewModel<CatedraticoDto>
{
    private readonly ICatedraticoService _service;
    private readonly IUsuarioService _usuarioService;

    public CatedraticoListViewModel(ICatedraticoService service, IUsuarioService usuarioService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _usuarioService = usuarioService;
        CargarAlIniciar();
    }

    protected override Task<List<CatedraticoDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(CatedraticoDto? existente) => new CatedraticoEditViewModel(_service, _usuarioService, existente);

    protected override Task DesactivarInternoAsync(CatedraticoDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(CatedraticoDto item) => $"{item.Nombres} {item.Apellidos}";
}
