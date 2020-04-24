using SMGApp.Domain.Models;
using SMGApp.Domain.Services;
using SMGApp.EntityFramework.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class ServiceViewModelFactory : ISMGAppViewModelFactory<ServiceViewModel>
    {
        private readonly ServiceItemsDataService _serviceItemService;

        public ServiceViewModelFactory(ServiceItemsDataService serviceItemService)
        {
            _serviceItemService = serviceItemService;
        }


        public ServiceViewModel CreateViewModel()
        {
            return new ServiceViewModel(_serviceItemService);
        }
    }
}