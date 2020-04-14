using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework
{
    public class SMGAppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
    }
}
