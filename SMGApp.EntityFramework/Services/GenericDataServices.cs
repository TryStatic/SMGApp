using System.Collections.Generic;
using System.Threading.Tasks;
using SMGApp.Domain.Services;

namespace SMGApp.EntityFramework.Services
{
    public class GenericDataServices<T> : IDataService<T>
    {
        public Task<IEnumerable<T>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Create(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Update(int id, T entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }

}
