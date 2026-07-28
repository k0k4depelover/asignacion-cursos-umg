using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class CatedraticoService : ICatedraticoService
    {
        private readonly AppContext _context;


            public CatedraticoService(AppContext context)
        {
            _context = context;
        }

        public async Task<List<Catedratico>> ObtenerTodosCatedraticosAsync()
        {
            return await _context.Catedratico.ToListAsync();
        }

        public async Task<Catedratico?> ObtenerCatedraticoPorIdAsync(int idCatedratico)
        {
            return await _context.FindAsync(idCatedratico);
        }

        public async Task<Catedratico> CrearCatedraticoAsync(Catedratico catedratico)
        {
            _context.Catedratico.Add(catedratico);
            await _context.SaveChangesAsync();
            return catedratico;
        }

        public async Task<bool> ActualizarCatedraticoAsync(int idCatedratico, Catedratico catedratico)
        {
            var catedraticoExistente = await _context.Catedratico.FindAsync(idCatedratico);
            if(catedraticoExistente == null)
            {
                return false; 
            }
            
            catedraticoExistente.CodigoCatedratico = catedratico.CodigoCatedratico;
            catedraticoExistente.DpiCatedratico = catedratico.DpiCatedratico;
            catedraticoExistente.NombresCatedratico = catedratico.NombresCatedratico;
            catedraticoExistente.ApellidosCatedratico = catedratico.ApellidosCatedratico;
            catedraticoExistente.TelefonoCatedratico = catedratico.TelefonoCatedratico;
            catedraticoExistente.ProfesionCatedratico = catedratico.ProfesionCatedratico;
            catedraticoExistente.EstadoCatedratico = catedratico.EstadoCatedratico;
            catedraticoExistente.IdUsuario = catedratico.IdUsuario;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCatedraticoAsync(int idCatedratico)
        {
            var catedraticoExistente = await _context.Catedratico.FindAsync(idCatedratico);
            if (catedraticoExistente == null) 
            { return false; }

            _context.Catedratico.Remove(catedraticoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
