namespace Asignacion.Wpf.Infrastructure;

public class ConexionConfig
{
    public string Host { get; set; } = "localhost";
    public int Puerto { get; set; } = 3306;
    public string BaseDatos { get; set; } = "asignacion_cursos";
    public string Usuario { get; set; } = "root";
    public string PasswordProtegida { get; set; } = "";

    public string ConnectionString(string passwordEnClaro) =>
        $"Server={Host};Port={Puerto};Database={BaseDatos};User={Usuario};Password={passwordEnClaro};";
}
