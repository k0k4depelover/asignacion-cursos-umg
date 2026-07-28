using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class DetalleAsignacionService : IDetalleAsignacionService
    {
        private readonly AppContext _context;

        public DetalleAsignacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleAsignacion>> ObtenerTodosDetallesAsignacionesAsync()
        {
            return await _context.DetallesAsignacion.ToListAsync();
        }

        public async Task<DetalleAsignacion?> ObtenerDetalleAsignacionPorIdAsync(int idDetalleAsignacion)
        {
            return await _context.DetallesAsignacion.FindAsync(idDetalleAsignacion);
        }

        public async Task<DetalleAsignacion> CrearDetalleAsignacionAsync(DetalleAsignacion detalleAsignacion)
        {
            _context.DetallesAsignacion.Add(detalleAsignacion);
            await _context.SaveChangesAsync();
            return detalleAsignacion;
        }

        public async Task<bool> ActualizarDetalleAsignacionAsync(int idDetalleAsignacion, DetalleAsignacion detalleAsignacion)
        {
            var detalleAsignacionExistente = await _context.DetallesAsignacion.FindAsync(idDetalleAsignacion);
            if (detalleAsignacionExistente == null)
            {
                return false;
            }

            detalleAsignacionExistente.IdDetalleAsignacion = detalleAsignacionExistente.IdDetalleAsignacion;
            detalleAsignacionExistente.EstadoDetalle = detalleAsignacion.EstadoDetalle;
            detalleAsignacionExistente.CostoLaboratorio = detalleAsignacion.CostoLaboratorio;
            detalleAsignacionExistente.NotaFinal = detalleAsignacion.NotaFinal;
            detalleAsignacionExistente.Resultado = detalleAsignacion.Resultado;
            detalleAsignacionExistente.FechaResultado = detalleAsignacion.FechaResultado;
            detalleAsignacionExistente.IdAsignacion = detalleAsignacion.IdAsignacion;
            detalleAsignacionExistente.IdSeccion = detalleAsignacion.IdSeccion;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarDetalleAsignacionAsync(int idDetalleAsignacion)
        {
            var detalleAsignacionExistente = await _context.DetallesAsignacion.FindAsync(idDetalleAsignacion);
            if (detalleAsignacionExistente == null)
            {
                return false;
            }
            _context.DetallesAsignacion.Remove(detalleAsignacionExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
