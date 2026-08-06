using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class AsignacionService: IAsignacionService
    {
        private readonly AppDbContext _context;
            
            public AsignacionService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Asignacion>> ObtenerTodasAsignacionesAsync()
        {
            return await _context.Asignacion.ToListAsync();
        }

        public async Task<Models.Asignacion?> ObtenerAsignacionPorIdAsync(int idAsignacion)
        {
            return await _context.Asignacion.FindAsync(idAsignacion);
        }

        public async Task<Models.Asignacion> CrearAsignacionAsync(Models.Asignacion asignacion)
        {
            await _context.Asignacion.AddAsync(asignacion);
            await _context.SaveChangesAsync();
            return asignacion;
        }

        public async Task<bool> ActualizarAsignacionAsync(int idAsignacion, Models.Asignacion asignacion)
        {
            var asignacionExistente = await _context.Asignacion.FindAsync(idAsignacion);
            if (asignacionExistente == null)
            {
                return false;
            }
            asignacionExistente.FechaAsignacion = asignacion.FechaAsignacion;
            asignacionExistente.SubTotalLaboratorios= asignacion.SubTotalLaboratorios;
            asignacionExistente.TotalPago = asignacion.TotalPago;
            asignacionExistente.EstadoAsignacion=asignacion.EstadoAsignacion;
            asignacionExistente.IdInscripcion = asignacion.IdInscripcion;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsignacionAsync(int idAsignacion)
        {
            var asignacionExistente = await _context.Asignacion.FindAsync(idAsignacion);
            if(asignacionExistente == null)
            {
                return false;
            }
            _context.Asignacion.Remove(asignacionExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
