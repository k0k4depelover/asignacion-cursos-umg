namespace Asignacion.Data.Common;

/// <summary>
/// Pricing assumptions the schema itself doesn't encode (no per-credit tuition column
/// exists anywhere) — kept as named constants so the enrollment workflow's cost math is
/// traceable to one place instead of magic numbers scattered through the service.
/// </summary>
public static class PreciosConstantes
{
    public const decimal CostoInscripcion = 250.00m;
    public const decimal CostoPorCredito = 150.00m;
}
