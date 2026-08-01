using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class EdificioListViewModel : CrudListViewModel<EdificioDto>
{
    private readonly IEdificioService _service;

    public EdificioListViewModel(IEdificioService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<EdificioDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(EdificioDto? existente) => new EdificioEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(EdificioDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(EdificioDto item) => item.Nombre;
}
