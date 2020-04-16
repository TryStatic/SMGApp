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
        private readonly SMGAppDbContextFactory _contextFactory;

        public GenericDataServices(SMGAppDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using SMGAppDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<T> entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public async Task<T> Get(int id)
        {
            await using SMGAppDbContext context = _contextFactory.CreateDbContext();
            T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.ID == id);
            return entity;
        }

        public async Task<T> Create(T entity)
        {
            await using SMGAppDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<T> createdEntity = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public async Task<T> Update(int id, T entity)
        {
            await using SMGAppDbContext context = _contextFactory.CreateDbContext();
            entity.ID = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            await using SMGAppDbContext context = _contextFactory.CreateDbContext();
            T entityToDelete = await context.Set<T>().FirstOrDefaultAsync(e => e.ID == id);
            if (entityToDelete == null) return false;
            context.Set<T>().Remove(entityToDelete);
            return true;
        }
    }

}
