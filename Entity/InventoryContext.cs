
using TevaInventorymanagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TevaInventorymanagementsystem.Entity
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
