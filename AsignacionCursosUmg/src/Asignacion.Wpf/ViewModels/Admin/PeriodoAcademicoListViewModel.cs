using Asignacion.Data.Common;
using Asignacion.Services.Programacion;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class PeriodoAcademicoListViewModel : CrudListViewModel<PeriodoAcademicoDto>
{
    private readonly IPeriodoAcademicoService _service;

    public PeriodoAcademicoListViewModel(IPeriodoAcademicoService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<PeriodoAcademicoDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(PeriodoAcademicoDto? existente) => new PeriodoAcademicoEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(PeriodoAcademicoDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(PeriodoAcademicoDto item) => item.Codigo;
}
