using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;
using AsignacionModel = Asignacion.Web.Models.Asignacion;

namespace Asignacion.Web.Services
{
    public class AsignacionService : IAsignacionService
    {
        private readonly AppDbContext _context;

        public AsignacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AsignacionModel>> ObtenerTodasAsignacionesAsync()
        {
            return await _context.Asignaciones.ToListAsync();
        }

        public async Task<AsignacionModel?> ObtenerAsignacionPorIdAsync(int idAsignacion)
        {
            return await _context.Asignaciones.FindAsync(idAsignacion);
        }

        public async Task<AsignacionModel> CrearAsignacionAsync(AsignacionModel asignacion)
        {
            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();
            return asignacion;
        }

        public async Task<bool> ActualizarAsignacionAsync(int idAsignacion, AsignacionModel asignacion)
        {
            var asignacionExistente = await _context.Asignaciones.FindAsync(idAsignacion);
            if (asignacionExistente == null)
            {
                return false;
            }

            asignacionExistente.IdAsignacion = asignacionExistente.IdAsignacion;
            asignacionExistente.FechaAsignacion = asignacion.FechaAsignacion;
            asignacionExistente.SubTotalLaboratorios = asignacion.SubTotalLaboratorios;
            asignacionExistente.TotalPago = asignacion.TotalPago;
            asignacionExistente.EstadoAsignacion = asignacion.EstadoAsignacion;
            asignacionExistente.IdInscripcion = asignacion.IdInscripcion;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsignacionAsync(int idAsignacion)
        {
            var asignacionExistente = await _context.Asignaciones.FindAsync(idAsignacion);
            if (asignacionExistente == null)
            {
                return false;
            }
            _context.Asignaciones.Remove(asignacionExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
