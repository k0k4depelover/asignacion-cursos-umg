using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class LaboratorioService : ILaboratorioService
    {
        private readonly AppDbContext _context;

        public LaboratorioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Laboratorio>> ObtenerTodosLaboratoriosAsync()
        {
            return await _context.Laboratorios.ToListAsync();
        }

        public async Task<Laboratorio?> ObtenerLaboratorioPorIdAsync(int idLaboratorio)
        {
            return await _context.Laboratorios.FindAsync(idLaboratorio);
        }

        public async Task<Laboratorio> CrearLaboratorioAsync(Laboratorio laboratorio)
        {
            _context.Laboratorios.Add(laboratorio);
            await _context.SaveChangesAsync();
            return laboratorio;
        }

        public async Task<bool> ActualizarLaboratorioAsync(int idLaboratorio, Laboratorio laboratorio)
        {
            var laboratorioExistente = await _context.Laboratorios.FindAsync(idLaboratorio);
            if (laboratorioExistente == null)
            {
                return false;
            }

            laboratorioExistente.IdLaboratorio = laboratorioExistente.IdLaboratorio;
            laboratorioExistente.NombreLaboratorio = laboratorio.NombreLaboratorio;
            laboratorioExistente.DescripcionLaboratorio = laboratorio.DescripcionLaboratorio;
            laboratorioExistente.EstadoLaboratorio = laboratorio.EstadoLaboratorio;
            laboratorioExistente.IdSalon = laboratorio.IdSalon;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarLaboratorioAsync(int idLaboratorio)
        {
            var laboratorioExistente = await _context.Laboratorios.FindAsync(idLaboratorio);
            if (laboratorioExistente == null)
            {
                return false;
            }
            _context.Laboratorios.Remove(laboratorioExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
