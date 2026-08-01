using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class CursoListViewModel : CrudListViewModel<CursoDto>
{
    private readonly ICursoService _service;

    public CursoListViewModel(ICursoService service, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        CargarAlIniciar();
    }

    protected override Task<List<CursoDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(CursoDto? existente) => new CursoEditViewModel(_service, existente);

    protected override Task DesactivarInternoAsync(CursoDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(CursoDto item) => item.Nombre;
}
