using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPensumCursoService
    {
        Task<List<PensumCurso>> ObtenerTodosPensumsCursosAsync();

        Task<PensumCurso?> ObtenerPensumCursoPorId(int idPensumCurso);

        Task<PensumCurso> CrearPensumCursoAsync(PensumCurso pensumCurso);

        Task<bool> ActualizarPensumCursoAsync(int idPensumCurso, PensumCurso pensumCurso);

        Task<bool> EliminarPensumCursoAsync(int idPensumCurso);
    }
}