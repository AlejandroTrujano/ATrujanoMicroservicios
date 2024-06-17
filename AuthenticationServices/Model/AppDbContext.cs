using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthenticationServices.Model
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //public virtual DbSet<Producto> Productos { get; set; }
    }
}
