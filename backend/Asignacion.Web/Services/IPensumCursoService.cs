using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPensumCursoService
    {
        Task<List<PensumCurso>> ObtenerTodosPensumCursosAsync();

        Task<PensumCurso?> ObtenerPensumCursoPorIdAsync(int idPensumCurso);

        Task<PensumCurso> CrearPensumCursoAsync(PensumCurso pensumCurso);

        Task<bool> ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso);

        Task<bool> EliminarPensumCursoAsync(int idPensumCurso);
    }
}