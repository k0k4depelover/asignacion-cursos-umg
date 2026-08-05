using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class FacultadListViewModel : CrudListViewModel<FacultadDto>
{
    private readonly IFacultadService _service;

    public FacultadListViewModel(IFacultadService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<FacultadDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(FacultadDto? existente) => new FacultadEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(FacultadDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(FacultadDto item) => item.Nombre;
}
