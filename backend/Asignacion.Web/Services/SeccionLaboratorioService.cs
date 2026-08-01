using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class SeccionLaboratorioService : ISeccionLaboratorioService
    {
        private readonly AppContext _context


            public SeccionLaboratorioService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<SeccionLaboratorio>> ObtenerTodasSeccionesLaboratoriosAsync()
        {
            return await _context.SeccionLaboratorio.ToListAsync();
        }

        public async Task<SeccionLaboratorio?> ObtenerSeccionLaboratorioPorIdAsync(int idSalon)
        {
            return await _context.SeccionLaboratorio.FindAsync(idSalon);
        }

        public async Task<SeccionLaboratorio> CrearSeccionLaboratorioAsync(SeccionLaboratorio seccionLaboratorio)
        {
            _context.SeccionLaboratorio.Add(seccionLaboratorio);
            await _context.SaveChangesAsync();
            return seccionLaboratorio;
        }

        public async Task<bool> ActualizarSeccionLaboratorioAsync(int idSeccionLaboratorio, SeccionLaboratorio seccionLaboratorio)
        {
            var seccionLaboratorioExistente = _context.SeccionLaboratorio.FindAsync(idSeccionLaboratorio);
            if (seccionLaboratorioExistente == null)
            {
                return false;
            }
            seccionLaboratorioExistente.IdSeccionLaboratorio = seccionLaboratorio.IdSeccionLaboratorio;
            seccionLaboratorioExistente.DiaSemana = seccionLaboratorio.DiaSemana;
            seccionLaboratorioExistente.HoraInicio = seccionLaboratorio.HoraInicio;
            seccionLaboratorioExistente.HoraFin = seccionLaboratorio.HoraFin;
            seccionLaboratorioExistente.CostoExtra = seccionLaboratorio.CostoExtra;
            seccionLaboratorioExistente.IdSeccion = seccionLaboratorio.IdSeccion;
            seccionLaboratorioExistente.IdLaboratorio = seccionLaboratorio.IdLaboratorio;
            return true;
        }

        public async Task<bool> EliminarSeccionLaboratorioAsync(int idSeccionLaboratorio)
        {
            var seccionLaboratorioExistente = _context.SeccionLaboratorio.FindAsync(idSeccionLaboratorio);
            if (seccionLaboratorioExistente == null)
            {
                return false;
            }
            _context.SeccionLaboratorio.Remove(seccionLaboratorioExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
