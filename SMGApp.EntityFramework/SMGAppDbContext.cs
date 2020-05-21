using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework
{
    public class SMGAppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<Guarantee> Guarantees { get; set; }

        public SMGAppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
