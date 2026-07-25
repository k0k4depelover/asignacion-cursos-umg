using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public interface ISeccionLaboratorioService
    {
        Task<List<SeccionLaboratorio>> ObtenerTodasSeccionesLaboratoriosAsync();

        Task<SeccionLaboratorio?> ObtenerSeccionLaboratorioPorId(int idSalon);

        Task<SeccionLaboratorio> CrearSeccionLaboratorioAsync(SeccionLaboratorio seccionLaboratorio);

        Task<bool> ActualizarSeccionLaboratorioAsync(int idSeccionLaboratorio, SeccionLaboratorio seccionLaboratorio);

        Task<bool> EliminarSeccionLaboratorioAsync(int idSeccionLaboratorio);
    }
}