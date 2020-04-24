using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

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

        public override Task<ServiceItem> Get(int id)
        {
            return base.Get(id);
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

    public class GenericDataServices<T> : IDataService<T> where T : DomainObject
    {
        protected readonly SMGAppDbContextFactory ContextFactory;

        public GenericDataServices(SMGAppDbContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            IEnumerable<T> entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public virtual async Task<T> Get(int id)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.ID == id);
            return entity;
        }

        public virtual async Task<T> Create(T entity)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            EntityEntry<T> createdEntity = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public virtual async Task<T> Update(int id, T entity)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            entity.ID = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> Delete(int id)
        {
            await using SMGAppDbContext context = ContextFactory.CreateDbContext();
            T entityToDelete = await context.Set<T>().FirstOrDefaultAsync(e => e.ID == id);
            if (entityToDelete == null) return false;
            context.Set<T>().Remove(entityToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
