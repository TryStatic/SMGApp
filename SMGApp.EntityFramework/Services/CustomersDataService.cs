using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework.Services
{
    public class CustomersDataService : GenericDataServices<Customer>
    {
        public CustomersDataService(SMGAppDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}