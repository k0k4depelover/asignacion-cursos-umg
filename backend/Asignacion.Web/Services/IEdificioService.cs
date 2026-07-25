using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IEdificioService
    {
        Task<List<Edificio>> ObtenerTodosEdificiosAsync();

        Task<Edificio?> ObtenerEdificioPorIdAsync(int idEdificio);

        Task<Edificio> CrearEdificionAsync(Edificio edificio);

        Task<bool> ActualizarEdificionAsync(int idEdificio, Edificio edificio);

        Task<bool> EliminarEdificioAsync(int idEdificio);
    }
}