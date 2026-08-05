using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class SeccionService : ISeccionService
    {
        private readonly AppContext _context


            public SeccionService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Seccion>> ObtenerTodasSeccionesAsync()
        {
            return await _context.Seccion.ToListAsync();
        }

        public async Task<Seccion?> ObtenerSeccionPorIdAsync(int idSalon)
        {
            return await _context.Seccion.FindAsync(idSalon);
        }

        public async Task<Seccion> CrearSeccionAsync(Seccion seccion)
        {
            _context.Seccion.Addsync(seccion);
            await _context.SaveChangesAsync();
            return seccion;
        }

        public async Task<bool> ActualizarSeccionAsync(int idSeccion, Seccion seccion)
        {
            var seccionExistente = await _context.Seccion.FindAsync(idSeccion);
            if (seccionExistente == null)
            {
                return false;
            }
            seccionExistente.IdSeccion = seccion.IdSeccion;
            seccionExistente.CodigoSeccion = seccion.CodigoSeccion;
            seccionExistente.Jornada = seccion.Jornada;
            seccionExistente.CupoMaximo = seccion.CupoMaximo;
            seccionExistente.EstadoSeccion = seccion.EstadoSeccion;
            seccionExistente.IdCurso = seccion.IdCurso;
            seccionExistente.IdPeriodo = seccion.IdPeriodo;
            seccionExistente.IdCatedratico = seccion.IdCatedratico;
            seccionExistente.IdSalon = seccion.IdSalon;
            return true;
        }

        public async Task<bool> EliminarSeccionAsync(int idSeccion)
        {
            var seccionExistente = _context.Seccion.FindAsync(idSeccion);
            if (seccionExistente == null)
            {
                return false;
            }
            _context.Seccion.Remove(seccionExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
