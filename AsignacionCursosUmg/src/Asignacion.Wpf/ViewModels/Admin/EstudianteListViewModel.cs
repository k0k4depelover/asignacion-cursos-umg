using Asignacion.Data.Common;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Personas;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels.Common;

namespace Asignacion.Wpf.ViewModels.Admin;

public class EstudianteListViewModel : CrudListViewModel<EstudianteDto>
{
    private readonly IEstudianteService _service;
    private readonly IUsuarioService _usuarioService;
    private readonly IPensumService _pensumService;

    public EstudianteListViewModel(IEstudianteService service, IUsuarioService usuarioService, IPensumService pensumService, IDialogService dialogService) : base(dialogService)
    {
        _service = service;
        _usuarioService = usuarioService;
        _pensumService = pensumService;
        CargarAlIniciar();
    }

    protected override Task<List<EstudianteDto>> ObtenerItemsAsync(bool incluirInactivos) => _service.GetAllAsync(incluirInactivos);

    protected override IEditDialogViewModel CrearViewModelEdicion(EstudianteDto? existente) => new EstudianteEditViewModel(_service, _usuarioService, _pensumService, existente);

    protected override Task DesactivarInternoAsync(EstudianteDto item) =>
        _service.SetEstadoAsync(item.Id, item.Estado != EstadoConstantes.Activo);

    protected override string DescribirItem(EstudianteDto item) => $"{item.Nombres} {item.Apellidos}";
}
