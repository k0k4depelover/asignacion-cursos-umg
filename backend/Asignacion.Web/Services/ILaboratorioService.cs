using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ILaboratorioService
    {
        Task<List<Laboratorio>> ObtenerTodosLaboratoriosAsync();

        Task<Laboratorio?> ObtenerLaboratorioPorId(int idLaboratorio);

        Task<Laboratorio> CrearLaboratorioAsync(Laboratorio laboratorio);

        Task<bool> ActualizarLaboratorioAsync(int idLaboratorio, Laboratorio laboratorio);

        Task<bool> EliminarLaboratorioAsync(int idLaboratorio);
    }
}