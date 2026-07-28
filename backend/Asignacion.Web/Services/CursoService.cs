using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;

        public CursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Curso>> ObtenerTodosCursosAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task<Curso?> ObtenerCursoPorIdAsync(int idCurso)
        {
            return await _context.Cursos.FindAsync(idCurso);
        }

        public async Task<Curso> CrearCursoAsync(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task<bool> ActualizarCursoAsync(int idCurso, Curso curso)
        {
            var cursoExistente = await _context.Cursos.FindAsync(idCurso);
            if (cursoExistente == null)
            {
                return false;
            }

            cursoExistente.IdCurso = cursoExistente.IdCurso;
            cursoExistente.CodigoCurso = curso.CodigoCurso;
            cursoExistente.NombreCurso = curso.NombreCurso;
            cursoExistente.CreditosCurso = curso.CreditosCurso;
            cursoExistente.RequiereLaboratorio = curso.RequiereLaboratorio;
            cursoExistente.EstadoCurso = curso.EstadoCurso;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCursoAsync(int idCurso)
        {
            var cursoExistente = await _context.Cursos.FindAsync(idCurso);
            if (cursoExistente == null)
            {
                return false;
            }
            _context.Cursos.Remove(cursoExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
