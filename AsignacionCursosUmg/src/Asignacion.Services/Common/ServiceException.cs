namespace Asignacion.Services.Common;

/// <summary>
/// A business-rule violation with a message meant to be shown directly to the user
/// (capacity full, schedule conflict, FK in use, duplicate code, etc.), as opposed to
/// an unexpected/technical exception that should bubble up as-is.
/// </summary>
public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }
}
