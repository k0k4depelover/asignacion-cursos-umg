using Asignacion.Data.Common;
using Asignacion.Services.Personas;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class RolListViewModel : CrudListViewModel<RolDto>
{
    private readonly IRolService _service;

    public RolListViewModel(IRolService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<RolDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(RolDto? existente) => new RolEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(RolDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(RolDto item) => item.Nombre;
}
