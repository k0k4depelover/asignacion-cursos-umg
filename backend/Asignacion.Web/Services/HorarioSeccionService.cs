using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class HorarioSeccionService : IHorarioSeccionService
    {
        private readonly AppDbContext _context;

        public HorarioSeccionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HorarioSeccion>> ObtenerTodosHorariosSeccionesAsync()
        {
            return await _context.HorariosSeccion.ToListAsync();
        }

        public async Task<HorarioSeccion?> ObtenerHorarioSeccionPorIdAsync(int idHorarioSeccion)
        {
            return await _context.HorariosSeccion.FindAsync(idHorarioSeccion);
        }

        public async Task<HorarioSeccion> CrearHorarioSeccionAsync(HorarioSeccion horarioSeccion)
        {
            _context.HorariosSeccion.Add(horarioSeccion);
            await _context.SaveChangesAsync();
            return horarioSeccion;
        }

        public async Task<bool> ActualizarHorarioSeccionAsync(int idHorarioSeccion, HorarioSeccion horarioSeccion)
        {
            var horarioSeccionExistente = await _context.HorariosSeccion.FindAsync(idHorarioSeccion);
            if (horarioSeccionExistente == null)
            {
                return false;
            }

            horarioSeccionExistente.IdHorario = horarioSeccionExistente.IdHorario;
            horarioSeccionExistente.DiaSemana = horarioSeccion.DiaSemana;
            horarioSeccionExistente.HoraInicio = horarioSeccion.HoraInicio;
            horarioSeccionExistente.HoraFin = horarioSeccion.HoraFin;
            horarioSeccionExistente.TipoSesion = horarioSeccion.TipoSesion;
            horarioSeccionExistente.IdSeccion = horarioSeccion.IdSeccion;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarHorarioSeccionAsync(int idHorarioSeccion)
        {
            var horarioSeccionExistente = await _context.HorariosSeccion.FindAsync(idHorarioSeccion);
            if (horarioSeccionExistente == null)
            {
                return false;
            }
            _context.HorariosSeccion.Remove(horarioSeccionExistente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
