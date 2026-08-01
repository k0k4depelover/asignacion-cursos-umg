using Asignacion.Services.Auth;
using Asignacion.Wpf.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Asignacion.Wpf.ViewModels;

public partial class LoginViewModel(IAuthService authService, IMessenger messenger) : ObservableObject
{
    [ObservableProperty]
    private string correo = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string? mensajeError;

    [ObservableProperty]
    private bool iniciandoSesion;

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        if (string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(Password))
        {
            MensajeError = "Ingrese su correo y contraseña.";
            return;
        }

        MensajeError = null;
        IniciandoSesion = true;
        try
        {
            var sesion = await authService.LoginAsync(Correo.Trim(), Password);
            if (sesion is null)
            {
                MensajeError = "Correo o contraseña incorrectos, o la cuenta está inactiva.";
                return;
            }

            Password = "";
            messenger.Send(new LoginSucceededMessage(sesion));
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudo iniciar sesión: {ex.Message}";
        }
        finally
        {
            IniciandoSesion = false;
        }
    }
}
