using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class SalonService : ISalonService
    {
        private readonly AppDbContext _context;

        public SalonService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Salon>> ObtenerTodosSalonesAsync()
        {
            return await _context.Salones.ToListAsync();
        }

        public async Task<Salon?> ObtenerSalonPorIdAsync(int idSalon)
        {
            return await _context.Salones.FindAsync(idSalon);
        }

        public async Task<Salon> CrearSalonAsync(Salon salon)
        {
            _context.Salones.Add(salon);
            await _context.SaveChangesAsync();
            return salon;
        }

        public async Task<bool> ActualizarSalonAsync(int idSalon, Salon salon)
        {
            var salonExistente = await _context.Salones.FindAsync(idSalon);
            if (salonExistente == null)
            {
                return false;
            }

            salonExistente.IdSalon = salonExistente.IdSalon;
            salonExistente.NombreSalon = salon.NombreSalon;
            salonExistente.CodigoSalon = salon.CodigoSalon;
            salonExistente.EstadoSalon = salon.EstadoSalon;
            salonExistente.CapacidadSalon = salon.CapacidadSalon;
            salonExistente.TipoEspacio = salon.TipoEspacio;
            salonExistente.NivelSalon = salon.NivelSalon;
            salonExistente.IdEdificio = salon.IdEdificio;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarSalonAsync(int idSalon)
        {
            var salonExistente = await _context.Salones.FindAsync(idSalon);
            if (salonExistente == null)
            {
                return false;
            }
            _context.Salones.Remove(salonExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
