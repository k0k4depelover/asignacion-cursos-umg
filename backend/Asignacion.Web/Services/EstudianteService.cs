using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly AppDbContext _context;


            public EstudianteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estudiante>> ObtenerTodosEstudiantesAsync()
        {
            return await _context.Estudiante.ToListAsync();
        }

        public async Task<Estudiante?> ObtenerEstudiantePorIdAsync(int idEstudiante)
        {
            return await _context.Estudiante.FindAsync(idEstudiante);
        }

        public async Task<Estudiante> CrearEstudianteAsync(Estudiante estudiante)
        {
            _context.Estudiante.Add(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task<bool> ActualizarEstudianteAsync(int idEstudiante, Estudiante estudiante)
        {
            var estudianteExistente = await _context.Estudiante.FindAsync(idEstudiante);
            if (estudianteExistente == null) {
                return false;
            }

            estudianteExistente.IdEstudiante = estudiante.IdEstudiante;
            estudianteExistente.NombresEstudiante = estudiante.NombresEstudiante;
            estudianteExistente.ApellidosEstudiante = estudiante.ApellidosEstudiante;
            estudianteExistente.CarnetEstudiante = estudiante.CarnetEstudiante;
            estudianteExistente.DpiEstudiante = estudiante.DpiEstudiante;
            estudianteExistente.FechaNacimientoEstudiante = estudiante.FechaNacimientoEstudiante;
            estudianteExistente.DireccionEstudiante = estudiante.DireccionEstudiante;
            estudianteExistente.TelefonoEstudiante = estudiante.TelefonoEstudiante;
            estudianteExistente.CicloEstudiante = estudiante.CicloEstudiante;
            estudianteExistente.IdUsuario = estudiante.IdUsuario;
            estudianteExistente.IdPensum = estudiante.IdPensum;
            estudianteExistente.EstadoEstudiante = estudiante.EstadoEstudiante;

            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> EliminarEstudianteAsync(int idEstudiante)
        {
            var estudianteExistente = await _context.Estudiante.FindAsync(idEstudiante);
            if (estudianteExistente == null)
            {
                return false;
            }

            _context.Estudiante.Remove(estudianteExistente);
            await _context.SaveChangesAsync(); 
            return true;
        }

    }
}
