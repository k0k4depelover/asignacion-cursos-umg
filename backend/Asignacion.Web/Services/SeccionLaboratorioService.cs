using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class SeccionLaboratorioService : ISeccionLaboratorioService
    {
        private readonly AppDbContext _context;

        public SeccionLaboratorioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeccionLaboratorio>> ObtenerTodasSeccionesLaboratoriosAsync()
        {
            return await _context.SeccionesLaboratorio.ToListAsync();
        }

        public async Task<SeccionLaboratorio?> ObtenerSeccionLaboratorioPorIdAsync(int idSeccionLaboratorio)
        {
            return await _context.SeccionesLaboratorio.FindAsync(idSeccionLaboratorio);
        }

        public async Task<SeccionLaboratorio> CrearSeccionLaboratorioAsync(SeccionLaboratorio seccionLaboratorio)
        {
            _context.SeccionesLaboratorio.Add(seccionLaboratorio);
            await _context.SaveChangesAsync();
            return seccionLaboratorio;
        }

        public async Task<bool> ActualizarSeccionLaboratorioAsync(int idSeccionLaboratorio, SeccionLaboratorio seccionLaboratorio)
        {
            var seccionLaboratorioExistente = await _context.SeccionesLaboratorio.FindAsync(idSeccionLaboratorio);
            if (seccionLaboratorioExistente == null)
            {
                return false;
            }

            seccionLaboratorioExistente.IdSeccionLaboratorio = seccionLaboratorioExistente.IdSeccionLaboratorio;
            seccionLaboratorioExistente.DiaSemana = seccionLaboratorio.DiaSemana;
            seccionLaboratorioExistente.HoraInicio = seccionLaboratorio.HoraInicio;
            seccionLaboratorioExistente.HoraFin = seccionLaboratorio.HoraFin;
            seccionLaboratorioExistente.CostoExtra = seccionLaboratorio.CostoExtra;
            seccionLaboratorioExistente.IdSeccion = seccionLaboratorio.IdSeccion;
            seccionLaboratorioExistente.IdLaboratorio = seccionLaboratorio.IdLaboratorio;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarSeccionLaboratorioAsync(int idSeccionLaboratorio)
        {
            var seccionLaboratorioExistente = await _context.SeccionesLaboratorio.FindAsync(idSeccionLaboratorio);
            if (seccionLaboratorioExistente == null)
            {
                return false;
            }
            _context.SeccionesLaboratorio.Remove(seccionLaboratorioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
