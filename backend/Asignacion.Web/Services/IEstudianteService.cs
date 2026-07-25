using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IEstudianteService
    {
        Task<List<Estudiante>> ObtenerTodosEstudiantesAsync();

        Task<Estudiante?> ObtenerEstudiantePorId(int idEstudiante);

        Task<Estudiante> CrearEstudianteAsync(Estudiante estudiante);

        Task<bool> ActualizarEstudianteAsync(int idEstudiante, Estudiante estudiante);

        Task<bool> EliminarEstudianteAsync(int idEstudiante);
    }
}