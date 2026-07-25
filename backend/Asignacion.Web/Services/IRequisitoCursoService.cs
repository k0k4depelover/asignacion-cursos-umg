using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IRequisitoCursoService
    {
        Task<List<RequisitoCurso>> ObtenerTodosRequisitosCursosAsync();

        Task<RequisitoCurso?> ObtenerRequisitoCursoPorId(int idRequisitoCurso);

        Task<RequisitoCurso> CrearRequisitoCursoAsync(RequisitoCurso requisitoCurso);

        Task<bool> ActualizarRequisitoCursoAsync(int idRequisitoCurso, RequisitoCurso requisitoCurso);

        Task<bool> EliminarRequisitoCursoAsync(int idRequisitoCurso);
    }
}