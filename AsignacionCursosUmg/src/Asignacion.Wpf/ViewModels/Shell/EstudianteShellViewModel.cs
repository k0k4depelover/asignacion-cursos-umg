using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.Messages;
using Asignacion.Wpf.ViewModels.Estudiante;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels.Shell;

public partial class EstudianteShellViewModel : ObservableObject
{
    private readonly IMessenger _messenger;

    public INavigationService Navigation { get; }
    public string NombreUsuario { get; }
    public List<MenuItemViewModel> MenuItems { get; }

    public EstudianteShellViewModel(INavigationService navigation, CurrentSessionService session, IMessenger messenger)
    {
        Navigation = navigation;
        _messenger = messenger;
        NombreUsuario = session.Current?.NombreUsuario ?? "";

        MenuItems =
        [
            new MenuItemViewModel("Mis cursos", new RelayCommand(() => Navigation.NavigateTo<MisCursosViewModel>())),
            new MenuItemViewModel("Inscribirme", new RelayCommand(() => Navigation.NavigateTo<InscripcionWizardViewModel>())),
            new MenuItemViewModel("Mi horario", new RelayCommand(() => Navigation.NavigateTo<MiHorarioViewModel>())),
            new MenuItemViewModel("Mis calificaciones", new RelayCommand(() => Navigation.NavigateTo<MisCalificacionesViewModel>())),
            new MenuItemViewModel("Mi pago", new RelayCommand(() => Navigation.NavigateTo<MiPagoViewModel>())),
        ];

        Navigation.NavigateTo<MisCursosViewModel>();
    }

    [RelayCommand]
    private void CerrarSesion() => _messenger.Send(new LogoutRequestedMessage());
}
