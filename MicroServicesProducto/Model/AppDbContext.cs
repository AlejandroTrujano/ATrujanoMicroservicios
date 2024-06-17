using Microsoft.EntityFrameworkCore;

namespace MicroServicesProducto.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        { 
        }

        public virtual DbSet<Producto> Productos { get; set; }
    }
}
