using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface IFacultadService
    {
        Task<List<Facultad>> ObtenerTodasFacultadesAsync();

        Task<Facultad?> ObtenerFacultadPorId(int idFacultad);

        Task<Facultad> CrearFacultadAsync(Facultad facultad);

        Task<bool> ActualizarFacultadAsync(int idFacultad, Facultad facultad);

        Task<bool> EliminarFacultadAsync(int idFacultad);
    }
}