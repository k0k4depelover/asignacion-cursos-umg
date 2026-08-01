namespace Asignacion.Services.Auth;

public interface IAuthService
{
    /// <summary>Returns null when credentials are invalid or the account is inactive.</summary>
    Task<SesionUsuario?> LoginAsync(string correo, string password);
}
