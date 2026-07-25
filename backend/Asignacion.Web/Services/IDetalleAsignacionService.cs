using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IDetalleAsignacionService
    {
        Task<List<DetalleAsignacion>> ObtenerTodosDetallesAsignacionesAsync();

        Task<DetalleAsignacion?> ObtenerDetalleAsignacionPorId(int idDetalleAsignacion);

        Task<DetalleAsignacion> CrearDetalleAsignacionAsync(DetalleAsignacion detalleAsignacion);

        Task<bool> ActualizarDetalleAsignacionAsync(int idDetalleAsignacion, DetalleAsignacion detalleAsignacion);

        Task<bool> EliminarDetalleAsignacionAsync(int idDetalleAsignacion);
    }
}