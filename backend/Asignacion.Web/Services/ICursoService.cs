using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ICursoService
    {
        Task<List<Curso>> ObtenerTodosCursosAsync();

        Task<Curso?> ObtenerCursoPorId(int idCurso);

        Task<Curso> CrearCursoAsync(Curso curso);

        Task<bool> ActualizarCursoAsync(int idCurso, Curso curso);

        Task<bool> EliminarCursoAsync(int idCurso);
    }
}