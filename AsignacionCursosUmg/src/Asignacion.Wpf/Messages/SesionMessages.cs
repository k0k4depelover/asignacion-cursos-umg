using Asignacion.Services.Auth;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Asignacion.Wpf.Messages;

public class LoginSucceededMessage(SesionUsuario sesion) : ValueChangedMessage<SesionUsuario>(sesion);

public class LogoutRequestedMessage;

public class NavegarASeccionEstudiantesMessage;

public class NavegarACalificacionesMessage;
