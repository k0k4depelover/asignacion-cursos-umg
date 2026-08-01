namespace Asignacion.Data.Common;

public static class EstadoConstantes
{
    public const string Activo = "activo";
    public const string Inactivo = "inactivo";

    public const string RolAdministrador = "Administrador";
    public const string RolEstudiante = "Estudiante";
    public const string RolCatedratico = "Catedratico";

    public const string ResultadoAprobado = "Aprobado";
    public const string ResultadoReprobado = "Reprobado";

    public const decimal NotaAprobatoria = 61m;

    public const int MaxCursosPorPeriodo = 5;

    public const string SolvenciaSolvente = "solvente";
    public const string SolvenciaMoroso = "moroso";

    public const string TipoRequisitoCursoAprobado = "curso_aprobado";
    public const string TipoRequisitoCreditosMinimos = "creditos_minimos";
}
