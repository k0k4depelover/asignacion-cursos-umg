using Asignacion.Services.Auth;

namespace Asignacion.Wpf.Infrastructure;

public class CurrentSessionService
{
    public SesionUsuario? Current { get; private set; }

    public event Action? SessionChanged;

    public void SetSession(SesionUsuario sesion)
    {
        Current = sesion;
        SessionChanged?.Invoke();
    }

    public void ClearSession()
    {
        Current = null;
        SessionChanged?.Invoke();
    }
}
