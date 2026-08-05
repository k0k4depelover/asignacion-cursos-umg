using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class CarreraService : ICarreraService
    {
        private readonly AppContext _context;

        public CarreraService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Carrera>> ObtenerTodasCarrerasAsync()
        {
            
            return await _context.Carrera.ToListAsync();
        }

        public async Task<Carrera?> ObtenerCarreraPorIdAsync(int idCarrera)
        {
            
            return await _context.Carrera.FindAsync(idCarrera);
        }

        public async Task<Carrera> CrearCarreraAsync(Carrera carrera)
        {
           
            await _context.Carrera.AddAsync(carrera);
            await _context.SaveChangesAsync();
            return carrera;
        }

        public async Task<bool> ActualizarCarreraAsync(int idCarrera, Carrera carrera)
        {
           
            var carreraExistente = await _context.Carrera.FindAsync(idCarrera);
            if (carreraExistente == null)
            {
                return false;
            }

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
            
            var carreraExistente = await _context.Carrera.FindAsync(idCarrera);
            if (carreraExistente == null)
            {
                return false;
            }

            _context.Carrera.Remove(carreraExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}