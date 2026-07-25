using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IHorarioSeccionService
    {
        Task<List<HorarioSeccion>> ObtenerTodosHorariosSeccionesAsync();

        Task<HorarioSeccion?> ObtenerHorarioSeccionPorIdAsync(int idHorarioSeccion);

        Task<HorarioSeccion> CrearHorarioSeccionAsync(HorarioSeccion horarioSeccion);

        Task<bool> ActualizarHorarioSeccionAsync(int idHorarioSeccion, HorarioSeccion horarioSeccion);

        Task<bool> EliminarHorarioSeccionAsync(int idHorarioSeccion);
    }
}