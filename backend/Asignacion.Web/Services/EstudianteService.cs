using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly AppContext _context;

        public EstudianteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estudiante>> ObtenerTodosEstudiantesAsync()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        public async Task<Estudiante?> ObtenerEstudiantePorIdAsync(int idEstudiante)
        {
            return await _context.Estudiantes.FindAsync(idEstudiante);
        }

        public async Task<Estudiante> CrearEstudianteAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task<bool> ActualizarEstudianteAsync(int idEstudiante, Estudiante estudiante)
        {
            var estudianteExistente = await _context.Estudiantes.FindAsync(idEstudiante);
            if (estudianteExistente == null)
            {
                return false;
            }

            estudianteExistente.IdEstudiante = estudianteExistente.IdEstudiante;
            estudianteExistente.NombresEstudiante = estudiante.NombresEstudiante;
            estudianteExistente.ApellidosEstudiante = estudiante.ApellidosEstudiante;
            estudianteExistente.CarnetEstudiante = estudiante.CarnetEstudiante;
            estudianteExistente.DpiEstudiante = estudiante.DpiEstudiante;
            estudianteExistente.FechaNacimientoEstudiante = estudiante.FechaNacimientoEstudiante;
            estudianteExistente.DireccionEstudiante = estudiante.DireccionEstudiante;
            estudianteExistente.TelefonoEstudiante = estudiante.TelefonoEstudiante;
            estudianteExistente.CicloEstudiante = estudiante.CicloEstudiante;
            estudianteExistente.EstadoEstudiante = estudiante.EstadoEstudiante;
            estudianteExistente.IdUsuario = estudiante.IdUsuario;
            estudianteExistente.IdPensum = estudiante.IdPensum;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarEstudianteAsync(int idEstudiante)
        {
            var estudianteExistente = await _context.Estudiantes.FindAsync(idEstudiante);
            if (estudianteExistente == null)
            {
                return false;
            }
            _context.Estudiantes.Remove(estudianteExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
