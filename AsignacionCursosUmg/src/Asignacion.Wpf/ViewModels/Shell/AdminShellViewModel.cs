using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.Messages;
using Asignacion.Wpf.ViewModels.Admin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels.Shell;

public partial class AdminShellViewModel : ObservableObject
{
    private readonly IMessenger _messenger;

    public INavigationService Navigation { get; }
    public string NombreUsuario { get; }
    public List<MenuItemViewModel> MenuItems { get; }

    public AdminShellViewModel(INavigationService navigation, CurrentSessionService session, IMessenger messenger)
    {
        Navigation = navigation;
        _messenger = messenger;
        NombreUsuario = session.Current?.NombreUsuario ?? "";

        MenuItems =
        [
            new MenuItemViewModel("Panel principal", new RelayCommand(() => Navigation.NavigateTo<AdminDashboardViewModel>())),
            new MenuItemViewModel("Facultades", new RelayCommand(() => Navigation.NavigateTo<FacultadListViewModel>())),
            new MenuItemViewModel("Carreras", new RelayCommand(() => Navigation.NavigateTo<CarreraListViewModel>())),
            new MenuItemViewModel("Pensums", new RelayCommand(() => Navigation.NavigateTo<PensumListViewModel>())),
            new MenuItemViewModel("Cursos", new RelayCommand(() => Navigation.NavigateTo<CursoListViewModel>())),
            new MenuItemViewModel("Edificios", new RelayCommand(() => Navigation.NavigateTo<EdificioListViewModel>())),
            new MenuItemViewModel("Salones", new RelayCommand(() => Navigation.NavigateTo<SalonListViewModel>())),
            new MenuItemViewModel("Laboratorios", new RelayCommand(() => Navigation.NavigateTo<LaboratorioListViewModel>())),
            new MenuItemViewModel("Períodos académicos", new RelayCommand(() => Navigation.NavigateTo<PeriodoAcademicoListViewModel>())),
            new MenuItemViewModel("Secciones", new RelayCommand(() => Navigation.NavigateTo<SeccionListViewModel>())),
            new MenuItemViewModel("Usuarios", new RelayCommand(() => Navigation.NavigateTo<UsuarioListViewModel>())),
            new MenuItemViewModel("Estudiantes", new RelayCommand(() => Navigation.NavigateTo<EstudianteListViewModel>())),
            new MenuItemViewModel("Catedráticos", new RelayCommand(() => Navigation.NavigateTo<CatedraticoListViewModel>())),
            new MenuItemViewModel("Roles", new RelayCommand(() => Navigation.NavigateTo<RolListViewModel>())),
            new MenuItemViewModel("Permisos", new RelayCommand(() => Navigation.NavigateTo<PermisoListViewModel>())),
            new MenuItemViewModel("Roles y permisos", new RelayCommand(() => Navigation.NavigateTo<RolPermisoMatrizViewModel>())),
        ];

        Navigation.NavigateTo<AdminDashboardViewModel>();
    }

    [RelayCommand]
    private void CerrarSesion() => _messenger.Send(new LogoutRequestedMessage());
}
