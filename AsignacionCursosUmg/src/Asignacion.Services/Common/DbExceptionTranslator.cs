using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Asignacion.Services.Common;

/// <summary>
/// Translates raw MySQL/EF exceptions into friendly ServiceExceptions, so the UI never
/// shows a raw MySqlException to the user (e.g. deleting a Curso that has active Secciones
/// hits the FK Restrict constraint and should read as "está en uso", not a stack trace).
/// </summary>
public static class DbExceptionTranslator
{
    private const int ErrForeignKeyRestrict = 1451;
    private const int ErrDuplicateEntry = 1062;

    public static async Task<T> RunAsync<T>(Func<Task<T>> action, string entidadNombre)
    {
        try
        {
            return await action();
        }
        catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.RowIsReferenced2 } or MySqlException { Number: ErrForeignKeyRestrict })
        {
            throw new ServiceException($"No se puede eliminar '{entidadNombre}': está en uso por otros registros.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry } or MySqlException { Number: ErrDuplicateEntry })
        {
            throw new ServiceException($"Ya existe un registro de '{entidadNombre}' con ese código/valor único.");
        }
    }

    public static async Task RunAsync(Func<Task> action, string entidadNombre)
    {
        await RunAsync(async () =>
        {
            await action();
            return true;
        }, entidadNombre);
    }
}
