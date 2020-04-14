using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMGApp.Domain.Services
{
    internal interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Create(T entity);
        Task<T> Update(int id, T entity);
        bool Delete(int id);
    }
}
