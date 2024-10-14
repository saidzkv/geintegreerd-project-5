using Microsoft.EntityFrameworkCore;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Models
{
    public class IMSDatabaseContext : DbContext
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
