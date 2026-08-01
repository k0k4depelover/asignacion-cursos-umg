using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
	public interface IAsignacionService {
		Task<List<Asignacion>> ObtenerTodasAsignacionesAsync();

		Task<Asignacion?> ObtenerAsignacionPorIdAsync(int idAsignacion);

		Task<Asignacion> CrearAsignacionAsync(Asignacion asignacion);

		Task<bool> ActualizarAsignacionAsync(int idAsignacion, Asignacion asignacion);

		Task<bool> EliminarAsignacionAsync(int idAsignacion);
	}
}