using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.EntityFramework.Services
{
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
