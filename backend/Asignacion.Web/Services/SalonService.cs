using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class SalonService : ISalonService
    {
        private readonly AppContext _context


            public SalonService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Salon>> ObtenerTodosSalonesAsync()
        {
            return await _context.Salon.ToListAsync();
        }

        public async Task<Salon?> ObtenerSalonPorIdAsync(int idSalon)
        {
            return await _context.Salon.FindAsync(idSalon);
        }

        public async Task<Salon> CrearSalonAsync(Salon salon)
        {
            _context.Salon.AddAsync(salon);
            await _context.SaveChangesAsync();
            return salon;
        }

        public async Task<bool> ActualizarSalonAsync(int idSalon, Salon salon)
        {
            var salonExistente = await _context.Salon.FindAsync(idSalon);
            if (salonExistente == null)
            {
                return false;
            }
            salonExistente.IdSalon = salon.IdSalon;
            salonExistente.NombreSalon = salon.NombreSalon;
            salonExistente.CodigoSalon = salon.CodigoSalon;
            salonExistente.EstadoSalon = salon.EstadoSalon;
            salonExistente.CapacidadSalon = salon.CapacidadSalon;
            salonExistente.TipoEspacio = salon.TipoEspacio;
            salonExistente.NivelSalon = salon.NivelSalon;
            salonExistente.IdEdificio = salon.IdEdificio;
            return true;
        }

        public async Task<bool> EliminarSalonAsync(int idSalon)
        {
            var salonExistente = _context.Salon.FindAsync(idSalon);
            if (salonExistente == null)
            {
                return false;
            }
            _context.Salon.Remove(salonExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
