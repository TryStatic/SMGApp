using System.Collections.Generic;
using System.Threading.Tasks;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework.Services
{
    public class CustomersDataService : GenericDataServices<Customer>
    {
        public CustomersDataService(SMGAppDbContextFactory contextFactory) : base(contextFactory)
        {

        }

        public override Task<IEnumerable<Customer>> GetAll()
        {
            return base.GetAll();
        }

        public override Task<Customer> Get(int id)
        {
            return base.Get(id);
        }

        public override Task<Customer> Create(Customer entity)
        {
            return base.Create(entity);
        }

        public override Task<Customer> Update(int id, Customer entity)
        {
            return base.Update(id, entity);
        }

        public override Task<bool> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}