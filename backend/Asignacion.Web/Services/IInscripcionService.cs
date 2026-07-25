using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IInscripcionService
    {
        Task<List<Inscripcion>> ObtenerTodasInscripcionesAsync();

        Task<Inscripcion?> ObtenerInscripcionPorIdAsync(int idInscripcion);

        Task<Inscripcion> CrearInscripcionAsync(Inscripcion inscripcion);

        Task<bool> ActualizarInscripcionAsync(int idInscripcion, Inscripcion inscripcion);

        Task<bool> EliminarInscripcionAsync(int idInscripcion);
    }
}