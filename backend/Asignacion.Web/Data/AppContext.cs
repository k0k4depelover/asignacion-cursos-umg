using Microsoft.EntityFrameworkCore;
using Asignacion.Web.Models;

namespace Asignacion.Web.Data
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }
        public DbSet<Asignacion> Asignaciones { get; set; }
    }
}