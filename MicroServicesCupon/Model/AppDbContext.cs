using Microsoft.EntityFrameworkCore;

namespace MicroServicesCupon.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public virtual DbSet<Cupon> Cupones { get; set; }


    }
}
