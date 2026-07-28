using Asignacion.Web.Models;
using AsignacionModel = Asignacion.Web.Models.Asignacion;

namespace Asignacion.Web.Services
{
	public interface IAsignacionService {
		Task<List<AsignacionModel>> ObtenerTodasAsignacionesAsync();

		Task<AsignacionModel?> ObtenerAsignacionPorIdAsync(int idAsignacion);

		Task<AsignacionModel> CrearAsignacionAsync(AsignacionModel asignacion);

		Task<bool> ActualizarAsignacionAsync(int idAsignacion, AsignacionModel asignacion);

		Task<bool> EliminarAsignacionAsync(int idAsignacion);
	}
}