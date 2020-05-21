using System.Collections.Generic;
using System.Threading.Tasks;
using SMGApp.Domain.Models;

namespace SMGApp.EntityFramework.Services
{
    public class GuaranteeDataService : GenericDataServices<Guarantee>
    {
        public GuaranteeDataService(SMGAppDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public override Task<IEnumerable<Guarantee>> GetAll()
        {
            return base.GetAll();
        }

        public override Task<Guarantee> Get(int id)
        {
            return base.Get(id);
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