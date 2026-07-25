using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Data;
using Asignacion.Web.Models;

namespace Asignacion.Web.Services
{
    public class AsignacionService : IAsignacionService
    {
        private readonly AppContext _context


            public AsignacionService(AppContext context)
        {
            _context = context;
        }

    }
}
