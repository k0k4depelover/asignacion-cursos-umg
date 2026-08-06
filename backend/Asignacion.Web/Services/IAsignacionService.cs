using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
	public interface IAsignacionService {
		Task<List<Models.Asignacion>> ObtenerTodasAsignacionesAsync();

		Task<Models.Asignacion?> ObtenerAsignacionPorIdAsync(int idAsignacion);

		Task<Models.Asignacion> CrearAsignacionAsync(Models.Asignacion asignacion);

		Task<bool> ActualizarAsignacionAsync(int idAsignacion, Models.Asignacion asignacion);

		Task<bool> EliminarAsignacionAsync(int idAsignacion);
	}
}
