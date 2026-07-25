using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ICatedraticoService
    {
        Task<List<Catedratico>> ObtenerTodosCatedraticosAsync();

        Task<Catedratico?> ObtenerCatedraticoPorIdAsync(int idCatedratico);

        Task<Catedratico> CrearCatedraticoAsync(Catedratico catedratico);

        Task<bool> ActualizarCatedraticoAsync(int idCatedratico, Catedratico catedratico);

        Task<bool> EliminarCatedraticoAsync(int idCatedratico);
    }
}