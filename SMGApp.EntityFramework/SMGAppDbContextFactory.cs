using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SMGApp.EntityFramework
{
    public class SMGAppDbContextFactory : IDesignTimeDbContextFactory<SMGAppDbContext>
    {
        public SMGAppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SMGAppDbContext> options = new DbContextOptionsBuilder<SMGAppDbContext>();
            options.UseSqlite("Data Source=SMGData.dat;");
            return new SMGAppDbContext(options.Options);
        }
    }
}
