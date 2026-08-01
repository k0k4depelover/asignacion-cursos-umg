using Asignacion.Wpf.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySqlConnector;

namespace Asignacion.Wpf.ViewModels;

/// <summary>
/// First-run (or connection-repair) dialog. Instantiated directly by App.xaml.cs before
/// the DI container exists, since the container itself depends on a working connection
/// string — see AppDbContextFactory/DI wiring in App.xaml.cs.
/// </summary>
public partial class ConexionSetupViewModel : ObservableObject
{
    [ObservableProperty]
    private string host = "localhost";

    [ObservableProperty]
    private int puerto = 3306;

    [ObservableProperty]
    private string baseDatos = "asignacion_cursos";

    [ObservableProperty]
    private string usuario = "root";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string? mensaje;

    [ObservableProperty]
    private bool conexionValidada;

    public bool GuardarYContinuar { get; private set; }

    public ConexionConfig ObtenerConfig() => new()
    {
        Host = Host,
        Puerto = Puerto,
        BaseDatos = BaseDatos,
        Usuario = Usuario
    };

    [RelayCommand]
    private async Task ProbarConexionAsync()
    {
        Mensaje = "Probando conexión...";
        ConexionValidada = false;
        try
        {
            var config = ObtenerConfig();
            await using var conexion = new MySqlConnection(config.ConnectionString(Password));
            await conexion.OpenAsync();
            ConexionValidada = true;
            Mensaje = "Conexión exitosa.";
        }
        catch (Exception ex)
        {
            Mensaje = $"No se pudo conectar: {ex.Message}";
        }
    }

    public event Action? SolicitarCierre;

    [RelayCommand]
    private void Continuar()
    {
        if (!ConexionValidada)
        {
            Mensaje = "Debe probar la conexión exitosamente antes de continuar.";
            return;
        }

        GuardarYContinuar = true;
        SolicitarCierre?.Invoke();
    }

    [RelayCommand]
    private void Cancelar()
    {
        GuardarYContinuar = false;
        SolicitarCierre?.Invoke();
    }
}
