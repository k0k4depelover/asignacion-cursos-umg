using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class SalonListViewModel : CrudListViewModel<SalonDto>
{
    private readonly ISalonService _service;
    private readonly IEdificioService _edificioService;

    public SalonListViewModel(ISalonService service, IEdificioService edificioService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _edificioService = edificioService;
        CargarAlIniciar();
    }

    protected override Task<List<SalonDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(SalonDto? existente) => new SalonEditViewModel(_service, _edificioService, existente);

    protected override Task DesactivarInternoAsync(SalonDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(SalonDto item) => item.Nombre;
}
