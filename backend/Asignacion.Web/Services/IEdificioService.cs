using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IEdificioService
    {
        Task<List<Edificio>> ObtenerTodosEdificiosAsync();

        Task<Edificio?> ObtenerEdificioPorIdAsync(int idEdificio);

        Task<Edificio> CrearEdificioAsync(Edificio edificio);

        Task<bool> ActualizarEdificioAsync(int idEdificio, Edificio edificio);

        Task<bool> EliminarEdificioAsync(int idEdificio);
    }
}