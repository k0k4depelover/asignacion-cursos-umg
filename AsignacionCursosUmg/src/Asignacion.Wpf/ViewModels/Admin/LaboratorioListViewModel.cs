using Asignacion.Data.Common;
using Asignacion.Services.Infraestructura;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class LaboratorioListViewModel : CrudListViewModel<LaboratorioDto>
{
    private readonly ILaboratorioService _service;
    private readonly ISalonService _salonService;

    public LaboratorioListViewModel(ILaboratorioService service, ISalonService salonService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _salonService = salonService;
        CargarAlIniciar();
    }

    protected override Task<List<LaboratorioDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(LaboratorioDto? existente) => new LaboratorioEditViewModel(_service, _salonService, existente);

    protected override Task DesactivarInternoAsync(LaboratorioDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(LaboratorioDto item) => item.Nombre;
}
