using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class DetalleAsignacionService : IDetalleAsignacionService
    {
        private readonly AppContext _context


            public DetalleAsignacionService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleAsignacion>> ObtenerTodosDetallesAsignacionesAsync()
        {
            return await _context.DetalleAsignacion.ToListAsync();
        }

        public async Task<DetalleAsignacion?> ObtenerDetalleAsignacionPorId(int idDetalleAsignacion)
        {
            return await _context.DetalleAsignacion.FindAsync(idDetalleAsignacion);
        }

        public async Task<DetalleAsignacion> CrearDetalleAsignacionAsync(DetalleAsignacion detalleAsignacion)
        {
            _context.DetalleAsignacion.Add(detalleAsignacion);
            await _context.SaveChangesAsync();
            return detalleAsignacion;
        }

        public async Task<bool> ActualizarDetalleAsignacionAsync(int idDetalleAsignacion, DetalleAsignacion detalleAsignacion)
        {
            var detalleAsignacionExistente = await _context.DetalleAsignacion.FindAsync(idDetalleAsignacion);
            if (detalleAsignacionExistente == null)
            {
                return false;
            }
            detalleAsignacionExistente.EstadoDetalle = detalleAsignacion.EstadoDetalle;
            detalleAsignacionExistente.CostoLaboratorio = detalleAsignacion.CostoLaboratorio;
            detalleAsignacionExistente.NotaFinal = detalleAsignacion.NotaFinal;
            detalleAsignacionExistente.Resultado = detalleAsignacion.Resultado;
            detalleAsignacionExistente.IdAsignacion = detalleAsignacion.IdAsignacion;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarDetalleAsignacionAsync(int idDetalleAsignacion)
        {
            var detalleAsignacionExistente = await _context.DetalleAsignacion.FindAsync(idDetalleAsignacion);
            if (detalleAsignacionExistente == null)
            {
                return false;
            }

            _context.DetalleAsignacion.Remove(detalleAsignacionExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
