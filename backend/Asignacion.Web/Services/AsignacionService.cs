using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class AsignacionService: IAsignacionService
    {
        private readonly AppContext _context
            
            public AsignacionService (AppContext context)
        {
            _context = context;
        }

        public async Task<List<Asignacion>> ObtenerTodasAsignacionesAsync()
        {
            return await _context.Asignacion.ToListAsync();
        }

        public async Task<Asignacion?> ObtenerAsignacionPorId(int idAsignacion)
        {
            return await _context.Asignacion.FindAsync(idAsignacion);
        }

        public async Task<Asignacion> CrearAsignacionAsync(Asignacion asignacion)
        {
            _context.Asignacion.Add(asignacion);
            await _context.SaveChangesAsync();
            return asignacion;
        }

        public async Task<bool> ActualizarAsignacionAsync(int idAsignacion, Asignacion asignacion)
        {
            var asignacionExistente = _context.Asignacion.FindAsync(idAsignacion);
            if (asignacionExistente == null)
            {
                return false;
            }
            asignacionExistente.FechaAsignacion = asignacion.FechaAsignacion;
            asignacionExistente.SubTotalLaboratorios= asignacion.SubTotalLaboratorios;
            asignacionExistente.TotalPago = asignacion.TotalPago;
            asignacionExistente.EstadoAsignacion=asignacion.EstadoAsignacion;
            asignacionExistente.IdInscripcion = asignacion.IdInscripcion;
            return true 
        }

        public async Task<bool> EliminarAsignacionAsync(int idAsignacion)
        {
            var asignacionExistente = _context.Asignacion.FindAsync(idAsignacion);
            if(asignacionExistente == null)
            {
                return false;
            }
            _context.Asignacion.Remove(asignacionExistente);
            await _context.SaveChangesAsync();
            return true
        }

    }
}
