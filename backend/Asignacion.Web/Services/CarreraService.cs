using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class CarreraService : ICarreraService
    {
        private readonly AppDbContext _context;

        public CarreraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Carrera>> ObtenerTodasCarrerasAsync()
        {
            return await _context.Carreras.ToListAsync();
        }

        public async Task<Carrera?> ObtenerCarreraPorIdAsync(int idCarrera)
        {
            return await _context.Carreras.FindAsync(idCarrera);
        }

        public async Task<Carrera> CrearCarreraAsync(Carrera carrera)
        {
            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();
            return carrera;
        }

        public async Task<bool> ActualizarCarreraAsync(int idCarrera, Carrera carrera)
        {
            var carreraExistente = await _context.Carreras.FindAsync(idCarrera);
            if (carreraExistente == null)
            {
                return false;
            }

            carreraExistente.IdCarrera = carreraExistente.IdCarrera;
            carreraExistente.NombreCarrera = carrera.NombreCarrera;
            carreraExistente.CodigoCarrera = carrera.CodigoCarrera;
            carreraExistente.TotalCiclos = carrera.TotalCiclos;
            carreraExistente.EstadoCarrera = carrera.EstadoCarrera;
            carreraExistente.IdFacultad = carrera.IdFacultad;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCarreraAsync(int idCarrera)
        {
            var carreraExistente = await _context.Carreras.FindAsync(idCarrera);
            if (carreraExistente == null)
            {
                return false;
            }
            _context.Carreras.Remove(carreraExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
