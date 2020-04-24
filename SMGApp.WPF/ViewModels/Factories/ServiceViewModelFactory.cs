using SMGApp.Domain.Models;
using SMGApp.Domain.Services;

namespace SMGApp.WPF.ViewModels.Factories
{
    public class ServiceViewModelFactory : ISMGAppViewModelFactory<ServiceViewModel>
    {
        private readonly IDataService<ServiceItem> _serviceItemService;

        public ServiceViewModelFactory(IDataService<ServiceItem> serviceItemService)
        {
            _serviceItemService = serviceItemService;
        }


        public ServiceViewModel CreateViewModel()
        {
            return new ServiceViewModel(_serviceItemService);
        }
    }
}