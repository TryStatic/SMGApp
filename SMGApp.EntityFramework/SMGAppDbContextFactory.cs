using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SMGApp.EntityFramework
{
    public class SMGAppDbContextFactory : IDesignTimeDbContextFactory<SMGAppDbContext>
    {
        private readonly string _path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public SMGAppDbContext CreateDbContext(string[] args = null)
        {
            DbContextOptionsBuilder<SMGAppDbContext> options = new DbContextOptionsBuilder<SMGAppDbContext>();
            options.UseSqlite($"Data Source={_path}\\SMGData.dat;");

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
