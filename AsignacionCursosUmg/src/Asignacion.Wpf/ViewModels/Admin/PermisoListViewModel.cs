using Asignacion.Services.Personas;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class PermisoListViewModel : CrudListViewModel<PermisoDto>
{
    private readonly IPermisoService _service;

    public PermisoListViewModel(IPermisoService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<PermisoDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync();

    protected override IEditDialogViewModel CrearViewModelEdicion(PermisoDto? existente) => new PermisoEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(PermisoDto item) => _service.DeleteAsync(item.Id);

    protected override string DescribirItem(PermisoDto item) => item.Nombre;
}
