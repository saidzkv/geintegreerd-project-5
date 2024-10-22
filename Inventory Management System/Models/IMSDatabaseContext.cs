using Microsoft.EntityFrameworkCore;
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Inventory_Management_System.Models
{
    public class IMSDatabaseContext : IdentityDbContext
    {
        public IMSDatabaseContext() 
        {
        }

        public IMSDatabaseContext(DbContextOptions<IMSDatabaseContext> options) : base(options) 
        {
        }

        public DbSet<Product> Products { get; set;}
        public DbSet<Stock> Stock { get; set; } = default!;
    }
}
