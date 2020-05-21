using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework.Services
{
    public class GuaranteeDataService : GenericDataServices<Guarantee>
    {
        public GuaranteeDataService(SMGAppDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public override async Task<IEnumerable<Guarantee>> GetAll()
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            IEnumerable<Guarantee> entities = await context.Set<Guarantee>().Include(b => b.Customer).ToListAsync();
            return entities;
        }

        public override async Task<Guarantee> Get(int id)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            Guarantee entry = await context.Guarantees.Include(b => b.Customer).FirstOrDefaultAsync(c => c.ID == id);
            return entry;
        }

        public override Task<Guarantee> Create(Guarantee entity)
        {
            return base.Create(entity);
        }

        public override Task<Guarantee> Update(int id, Guarantee entity)
        {
            return base.Update(id, entity);
        }

        public override Task<bool> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}