using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework.Services
{
    public class ServiceItemsDataService : GenericDataServices<ServiceItem>
    {
        public ServiceItemsDataService(SMGAppDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public override async Task<IEnumerable<ServiceItem>> GetAll()
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            IEnumerable<ServiceItem> entities = await context.Set<ServiceItem>().Include(b => b.Customer).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<ServiceItem>> GetByCustomer(int customerID)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            IEnumerable<ServiceItem> entities = await context.Set<ServiceItem>().Include(b => b.Customer).Where(c => c.Customer.ID == customerID).ToListAsync();
            return entities;
        }

        public override async Task<ServiceItem> Get(int id)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            ServiceItem entity = await context.Set<ServiceItem>().Include(c => c.Customer).FirstOrDefaultAsync(e => e.ID == id);
            return entity;
        }

        public override Task<ServiceItem> Create(ServiceItem entity)
        {
            return base.Create(entity);
        }

        public override Task<ServiceItem> Update(int id, ServiceItem entity)
        {
            return base.Update(id, entity);
        }

        public override Task<bool> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}