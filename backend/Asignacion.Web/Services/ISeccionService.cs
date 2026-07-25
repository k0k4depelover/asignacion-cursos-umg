using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ISeccionService
    {
        Task<List<Seccion>> ObtenerTodasSeccionesAsync();

        Task<Seccion?> ObtenerSeccionPorId(int idSalon);

        Task<Seccion> CrearSeccionAsync(Seccion seccion);

        Task<bool> ActualizarSeccionAsync(int idSeccion, Seccion seccion);

        Task<bool> EliminarSeccionAsync(int idSeccion);
    }
}