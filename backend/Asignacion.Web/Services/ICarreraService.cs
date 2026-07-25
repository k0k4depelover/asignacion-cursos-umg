using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ICarreraService
    {
        Task<List<Carrera>> ObtenerTodasCarrerasAsync();

        Task<Carrera?> ObtenerCarreraPorIdAsync(int idCarrera);

        Task<Carrera> CrearCarreraAsync(Carrera carrera);

        Task<bool> ActualizarCarreraAsync(int idCarrera, Carrera carrera);

        Task<bool> EliminarCarreraAsync(int idCarrera);
    }
}