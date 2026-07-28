using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly AppContext _context;

        public InscripcionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inscripcion>> ObtenerTodasInscripcionesAsync()
        {
            return await _context.Inscripciones.ToListAsync();
        }

        public async Task<Inscripcion?> ObtenerInscripcionPorIdAsync(int idInscripcion)
        {
            return await _context.Inscripciones.FindAsync(idInscripcion);
        }

        public async Task<Inscripcion> CrearInscripcionAsync(Inscripcion inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
            return inscripcion;
        }

        public async Task<bool> ActualizarInscripcionAsync(int idInscripcion, Inscripcion inscripcion)
        {
            var inscripcionExistente = await _context.Inscripciones.FindAsync(idInscripcion);
            if (inscripcionExistente == null)
            {
                return false;
            }

            inscripcionExistente.IdInscripcion = inscripcionExistente.IdInscripcion;
            inscripcionExistente.FechaInscripcion = inscripcion.FechaInscripcion;
            inscripcionExistente.CostoInscripcion = inscripcion.CostoInscripcion;
            inscripcionExistente.EstadoInscripcion = inscripcion.EstadoInscripcion;
            inscripcionExistente.MontoMensual = inscripcion.MontoMensual;
            inscripcionExistente.CicloInscrito = inscripcion.CicloInscrito;
            inscripcionExistente.EstadoSolvencia = inscripcion.EstadoSolvencia;
            inscripcionExistente.IdEstudiante = inscripcion.IdEstudiante;
            inscripcionExistente.IdPeriodoAcademico = inscripcion.IdPeriodoAcademico;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarInscripcionAsync(int idInscripcion)
        {
            var inscripcionExistente = await _context.Inscripciones.FindAsync(idInscripcion);
            if (inscripcionExistente == null)
            {
                return false;
            }
            _context.Inscripciones.Remove(inscripcionExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
