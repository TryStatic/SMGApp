using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SMGApp.EntityFramework
{
    public class SMGAppDbContextFactory : IDesignTimeDbContextFactory<SMGAppDbContext>
    {
        public SMGAppDbContext CreateDbContext(string[] args = null)
        {
            DbContextOptionsBuilder<SMGAppDbContext> options = new DbContextOptionsBuilder<SMGAppDbContext>();
            options.UseSqlite("Data Source=SMGData.dat;");
            return new SMGAppDbContext(options.Options);
        }

        public static async Task MigrateIfNeeded()
        {
            SMGAppDbContextFactory factory = new SMGAppDbContextFactory();
            await using SMGAppDbContext context = factory.CreateDbContext();
            await context.Database.MigrateAsync();
        }
    }
}
