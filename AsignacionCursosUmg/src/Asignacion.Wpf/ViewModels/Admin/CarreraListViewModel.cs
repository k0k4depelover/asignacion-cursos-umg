using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class CarreraListViewModel : CrudListViewModel<CarreraDto>
{
    private readonly ICarreraService _service;
    private readonly IFacultadService _facultadService;

    public CarreraListViewModel(ICarreraService service, IFacultadService facultadService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _facultadService = facultadService;
        CargarAlIniciar();
    }

    protected override Task<List<CarreraDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(CarreraDto? existente) => new CarreraEditViewModel(_service, _facultadService, existente);

    protected override Task DesactivarInternoAsync(CarreraDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(CarreraDto item) => item.Nombre;
}
