using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ISalonService
    {
        Task<List<Salon>> ObtenerTodosSalonesAsync();

        Task<Salon?> ObtenerSalonPorId(int idSalon);

        Task<Salon> CrearSalonAsync(Salon salon);

        Task<bool> ActualizarSalonAsync(int idSalon, Salon salon);

        Task<bool> EliminarSalonAsync(int idSalon);
    }
}