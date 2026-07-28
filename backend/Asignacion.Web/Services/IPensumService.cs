using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IPensumService
    {
        Task<List<Pensum>> ObtenerTodosPensumsAsync();

        Task<Pensum?> ObtenerPensumPorIdAsync(int idPensum);

        Task<Pensum> CrearPensumAsync(Pensum pensum);

        Task<bool> ActualizarPensumAsync(int idPensum, Pensum pensum);

        Task<bool> EliminarPensumAsync(int idPensum);
    }
}