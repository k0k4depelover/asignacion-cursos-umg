using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class LaboratorioService : ILaboratorioService
    {
        private readonly AppContext _context;


            public LaboratorioService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Laboratorio>> ObtenerTodosLaboratoriosAsync()
        {
            return await _context.Laboratorio.ToListAsync();
        }

        public async Task<Laboratorio?> ObtenerLaboratorioPorIdAsync(int idLaboratorio)
        {
            return await _context.Laboratorio.FindAsync(idLaboratorio);
        }

        public async Task<Laboratorio> CrearLaboratorioAsync(Laboratorio laboratorio)
        {
            _context.Laboratorio.AddAsync(laboratorio);
            await _context.SaveChangesAsync();
            return laboratorio;
        }

        public async Task<bool> ActualizarLaboratorioAsync(int idLaboratorio, Laboratorio laboratorio)
        {
            var laboratorioExistente = _context.Laboratorio.FindAsync(idLaboratorio);
            if (laboratorioExistente == null)
            {
                return false;
            }

            laboratorioExistente.IdLaboratorio = laboratorio.IdLaboratorio;
            laboratorioExistente.NombreLaboratorio = laboratorio.NombreLaboratorio;
            laboratorioExistente.DescripcionLaboratorio = laboratorio.DescripcionLaboratorio;
            laboratorioExistente.EstadoLaboratorio = laboratorio.EstadoLaboratorio;
            laboratorioExistente.IdSalon = laboratorio.IdSalon;
            return true;
        }

        public async Task<bool> EliminarLaboratorioAsync(int idLaboratorio)
        {
            var laboratorioExistente = await _context.Laboratorio.FindAsync(idLaboratorio);
            if (laboratorioExistente == null)
            {
                return false;
            }

            _context.Laboratorio.Remove(laboratorioExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
