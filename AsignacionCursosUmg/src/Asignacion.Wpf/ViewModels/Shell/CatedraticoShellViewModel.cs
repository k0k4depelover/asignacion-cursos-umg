using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.Messages;
using Asignacion.Wpf.ViewModels.Catedratico;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels.Shell;

public partial class CatedraticoShellViewModel : ObservableObject,
    IRecipient<NavegarASeccionEstudiantesMessage>, IRecipient<NavegarACalificacionesMessage>
{
    private readonly IMessenger _messenger;

    public INavigationService Navigation { get; }
    public string NombreUsuario { get; }
    public List<MenuItemViewModel> MenuItems { get; }

    public CatedraticoShellViewModel(INavigationService navigation, CurrentSessionService session, IMessenger messenger)
    {
        Navigation = navigation;
        _messenger = messenger;
        NombreUsuario = session.Current?.NombreUsuario ?? "";

        MenuItems =
        [
            new MenuItemViewModel("Mis secciones", new RelayCommand(() => Navigation.NavigateTo<MisSeccionesViewModel>())),
        ];

        messenger.RegisterAll(this);
        Navigation.NavigateTo<MisSeccionesViewModel>();
    }

    public void Receive(NavegarASeccionEstudiantesMessage message) => Navigation.NavigateTo<SeccionEstudiantesViewModel>();

    public void Receive(NavegarACalificacionesMessage message) => Navigation.NavigateTo<CalificacionesViewModel>();

    [RelayCommand]
    private void CerrarSesion() => _messenger.Send(new LogoutRequestedMessage());
}
