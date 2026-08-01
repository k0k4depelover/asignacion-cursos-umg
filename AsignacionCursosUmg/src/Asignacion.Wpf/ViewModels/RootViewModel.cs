using Asignacion.Data.Common;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.Messages;
using Asignacion.Wpf.ViewModels.Catedratico;
using Asignacion.Wpf.ViewModels.Estudiante;
using Asignacion.Wpf.ViewModels.Shell;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Asignacion.Wpf.ViewModels;

/// <summary>
/// The single window's content root: swaps between LoginViewModel and one of the three
/// role Shell ViewModels, driven by LoginSucceededMessage/LogoutRequestedMessage.
/// </summary>
public partial class RootViewModel : ObservableObject, IRecipient<LoginSucceededMessage>, IRecipient<LogoutRequestedMessage>
{
    private readonly IServiceProvider _provider;
    private readonly CurrentSessionService _session;

    [ObservableProperty]
    private object? currentViewModel;

    public RootViewModel(IServiceProvider provider, CurrentSessionService session, IMessenger messenger)
    {
        _provider = provider;
        _session = session;
        messenger.RegisterAll(this);
    }

    public void MostrarLogin()
    {
        CurrentViewModel = _provider.GetRequiredService<LoginViewModel>();
    }

    public void Receive(LoginSucceededMessage message)
    {
        _session.SetSession(message.Value);

        CurrentViewModel = message.Value.RolNombre switch
        {
            EstadoConstantes.RolAdministrador => _provider.GetRequiredService<AdminShellViewModel>(),
            EstadoConstantes.RolEstudiante => _provider.GetRequiredService<EstudianteShellViewModel>(),
            EstadoConstantes.RolCatedratico => _provider.GetRequiredService<CatedraticoShellViewModel>(),
            _ => CurrentViewModel
        };
    }

    public void Receive(LogoutRequestedMessage message)
    {
        _session.ClearSession();
        MostrarLogin();
    }
}
