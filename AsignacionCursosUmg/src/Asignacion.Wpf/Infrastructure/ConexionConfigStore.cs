using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Asignacion.Wpf.Infrastructure;

/// <summary>
/// Per-machine connection settings, stored outside the repo/install folder under
/// %LOCALAPPDATA%, with the password protected via Windows DPAPI (current-user scope) —
/// replaces the old approach of a plaintext DB password committed to appsettings.json.
/// </summary>
public static class ConexionConfigStore
{
    private static readonly string CarpetaConfig =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AsignacionCursosUMG");

    private static readonly string ArchivoConfig = Path.Combine(CarpetaConfig, "connection.json");

    public static bool Existe() => File.Exists(ArchivoConfig);

    public static (ConexionConfig Config, string Password)? Cargar()
    {
        if (!File.Exists(ArchivoConfig))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(ArchivoConfig);
            var config = JsonSerializer.Deserialize<ConexionConfig>(json);
            if (config is null)
            {
                return null;
            }

            var protegido = Convert.FromBase64String(config.PasswordProtegida);
            var claro = ProtectedData.Unprotect(protegido, null, DataProtectionScope.CurrentUser);
            return (config, Encoding.UTF8.GetString(claro));
        }
        catch
        {
            return null;
        }
    }

    public static void Guardar(ConexionConfig config, string passwordEnClaro)
    {
        Directory.CreateDirectory(CarpetaConfig);

        var protegido = ProtectedData.Protect(Encoding.UTF8.GetBytes(passwordEnClaro), null, DataProtectionScope.CurrentUser);
        config.PasswordProtegida = Convert.ToBase64String(protegido);

        File.WriteAllText(ArchivoConfig, JsonSerializer.Serialize(config));
    }
}
